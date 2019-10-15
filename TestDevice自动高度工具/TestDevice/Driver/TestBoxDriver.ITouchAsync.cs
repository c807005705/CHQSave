// Copyright 2011 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.ComponentModel;
using TestDevice;
using Mobot;
using Motor;

namespace Driver
{
    partial class TestBoxDriver : ITouchAsync
    {
        /// <summary>
        /// 移到触屏指定的坐标。
        /// </summary>
        public void TouchMoveAsync(int pixelX, int pixelY)
        {
            double x, y;
            this.PointToMotor(pixelX, pixelY, out x, out y);
            try
            {
                this.BatchActions(new BatchMoveXY(x, y));
            }
            catch (MotorException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("移动失败。" + ex.Message, ex);
            }
        }
        /// <summary>
        /// 移动Z（触屏坐标系）
        /// </summary>
        public void TouchMoveZAsync(double? touchZ = null)
        {
            if (touchZ == null)
                touchZ = TouchPadClickZ == null ? StylusLocationZ : TouchPadClickZ.Value;
            try
            {
                this.BatchActions(new BatchMoveZ(touchZ ?? 0));
            }
            catch (MotorException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("移动Z失败。" + ex.Message, ex);
            }
        }
        /// <summary>
        /// 触屏操作
        /// </summary>
        public void TouchClickAsync(int pixelX, int pixelY, int duration = 100)
        {
            double touchZ = TouchPadClickZ == null ? StylusLocationZ : TouchPadClickZ.Value;
            TouchClickAsync(touchZ, pixelX, pixelY, 1, duration);
        }
        /// <summary>
        /// 触屏操作
        /// </summary>
        public void TouchClickAsync(double touchZ, int pixelX, int pixelY, int clicks, int duration = 100)
        {
            double x, y;
            this.PointToMotor(pixelX, pixelY, out x, out y);
            try
            {
                this.KeyPressAsync(x, y, touchZ, clicks, duration);
            }
            catch (MotorException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("按键失败。" + ex.Message, ex);
            }
        }
        /// <summary>
        /// 触屏拖动。
        /// </summary>
        public void TouchDragAsync(Point start, Point end, int duration = 100)
        {
            double touchZ = TouchPadClickZ == null ? StylusLocationZ : TouchPadClickZ.Value;
            TouchDragAsync(touchZ, new List<Point> { start, end }, duration);
        }
        /// <summary>
        /// 触屏拖动（指定触屏点击高度与拖动速度）
        /// </summary>
        public void TouchDragAsync(double touchZ, IList<Point> points, int duration)
        {
            Point2D[] result = new Point2D[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                Point item = points[i];

                double x, y;
                this.PointToMotor(item.X, item.Y, out x, out y);

                result[i] = new Point2D(x, y);
            }
            try
            {
                this.StylusDragAsync(touchZ, result, duration);
            }
            catch (MotorException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("拖动失败。" + ex.Message, ex);
            }
        }
        public void KeyPressAsync(string key, int duration = 0)
        {
            if (!key.StartsWith("KEY"))
            {
                key = string.Format("KEY.{0}", key);
            }
            if (this.Keys.ContainsKey(key))
            {
                this.StylusPressAsync(Keys[key], 1, duration);
            }
            else
                throw new ArgumentException("不支持该键。", "key");
        }
        public void KeyPressAsync(double x, double y, double z, int clicks, int duration)
        {
            this.StylusPressAsync(new MobileKeyInfo(x, y, z), clicks, duration);
        }
        public void KeyPressAsync(MobileKeyInfo keyInfo, int clicks, int duration)
        {
            this.StylusPressAsync(keyInfo, clicks, duration);
        }
        public void KeyPressAsync(List<MobileKeyInfo> list, int clicks, int duration)
        {
            List<IMotorAction> execList = new List<IMotorAction>();
            for (int i = 0; i < list.Count; i++)
            {
                List<IMotorAction> temp = this.StylusPressList(list[i], duration);
                execList.AddRange(temp);
            }
            this.BatchActionsAsync(execList);
        }

        /// <summary>
        /// 拖动
        /// </summary>
        private void StylusDragAsync(double touchZ, IList<Point2D> points, int duration)
        {
            try
            {
                if (points.Count < 2) return;

                //未按下的位置
                double zInnerPressed = (this.QuickPointZ != null) ? this.QuickPointZ.Value : 50;//连续点击高度
                Point2D xyPressed = new Point2D(points[0].X, points[0].Y);

                //拖动动作
                List<IMotorAction> list = new List<IMotorAction>();

                //XY移动到目标位置，Z到初始高度或安全高度。未按下状态
                list.Add(new BatchMoveXY(xyPressed.X, xyPressed.Y));

                //第一点
                list.Add(new BatchMoveZ(touchZ));
                list.Add(new BatchSleep(duration));

                //第二点
                list.Add(new BatchMoveXY(points[points.Count - 1].X, points[points.Count - 1].Y));
                //list.Add(new BatchWorkZ(zNotPressed));

                BatchActionsAsync(list);
            }
            catch (MotorException ex)
            {
                log.Error(ex);
                throw ex;

            }
        }
        private List<IMotorAction> StylusPressList(MobileKeyInfo keyInfo, int duration)
        {
            //KeyPress 动作
            MobileKeyPressArgs pressArgs = keyInfo.PressArgs;

            //按下的位置
            Point3D locationPressed = new Point3D(
                keyInfo.Location.X + ((pressArgs.Axis == Axis.X) ? pressArgs.Depth : 0),
                keyInfo.Location.Y + ((pressArgs.Axis == Axis.Y) ? pressArgs.Depth : 0),
                keyInfo.Location.Z + ((pressArgs.Axis == Axis.Z) ? pressArgs.Depth : 0));

            //带有移动速度的按键
            bool isDepthPress = (
                pressArgs.Speed > 0
                && pressArgs.Depth != 0
                && HardwareVersion.Major != 1); //一代测试盒不支持批处理中改变速度

            //未按下的位置
            Point3D locationNotPressed = keyInfo.Location;
            Point3D locationInnerPressed = keyInfo.Location;
            locationInnerPressed.Z = (this.QuickPointZ != null) ? this.QuickPointZ.Value : 50;//连续点击高度

            List<IMotorAction> list = new List<IMotorAction>();


            //XY移动到目标位置，Z到初始高度或安全高度。未按下状态
            list.Add(new BatchMoveXY(locationNotPressed.X, locationNotPressed.Y));

            if (keyInfo.PressArgs.Axis == Axis.Z)
            {
                if (isDepthPress)
                {
                    Point3 speed = MoveSpeed(pressArgs);
                    list.Add(new BatchMotorSpeed(speed));
                }
                //按下但不抬起
                list.Add(new BatchMoveZ(locationPressed.Z));
                list.Add(new BatchSleep(duration));

                if (isDepthPress)
                {
                    list.Add(new BatchMotorSpeedReset());
                }
            }
            else
            {
                //按下回原xy位但不抬起
                list.Add(new BatchMoveZ(locationNotPressed.Z));

                if (locationNotPressed.X != locationPressed.X || locationNotPressed.Y != locationPressed.Y)
                {
                    if (isDepthPress)
                    {
                        Point3 speed = MoveSpeed(pressArgs);
                        list.Add(new BatchMotorSpeed(speed));
                    }
                    list.Add(new BatchMoveXY(locationPressed.X, locationPressed.Y));
                    list.Add(new BatchSleep(duration));
                    list.Add(new BatchMoveXY(locationNotPressed.X, locationNotPressed.Y));

                    if (isDepthPress)
                    {
                        list.Add(new BatchMotorSpeedReset());
                    }
                }
                else
                {
                    list.Add(new BatchSleep(duration));
                }
            }

            if (this.QuickPointZ == null)
                list.Add(new BatchMoveSafeZ());
            else
                list.Add(new BatchMoveZ(locationInnerPressed.Z));

            return list;
        }
        private void StylusPressAsync(MobileKeyInfo keyInfo, int clicks, int duration)
        {
            List<IMotorAction> list = new List<IMotorAction>();
            List<IMotorAction> temp = StylusPressList(keyInfo, duration);
            for (int i = 0; i < clicks; i++)
            {
                list.AddRange(temp);
            }
            BatchActionsAsync(list);
        }
        private void BatchActions(params IMotorAction[] actions)
        {
            List<IMotorCommand> commands = new List<IMotorCommand>();
            foreach (IMotorAction item in actions)
            {
                if (item is BatchSleep)
                {
                    BatchSleep temp = item as BatchSleep;

                    for (int i = 0; i < temp.MS / 20000; i++)
                    {
                        BatchSleep t = new BatchSleep(20000);
                        commands.Add(t.ToMotorCommand(this.settings));
                    }

                    if (temp.MS % 20000 > 0)
                    {
                        BatchSleep t = new BatchSleep(temp.MS % 20000);
                        commands.Add(t.ToMotorCommand(this.settings));
                    }

                    continue;
                }
                commands.Add(item.ToMotorCommand(this.settings));
            }
            driver.Batch(commands.ToArray());
        }

        #region 异步执行
        /// <summary>
        /// 等待按的键。
        /// </summary>
        Queue<ActionBatch> queueKeys = new Queue<ActionBatch>();
        BackgroundWorker bw = null;
        private bool Moving;
        public void BatchActionsAsync(IList<IMotorAction> actions)
        {
            if (actions == null)
                return;
            lock (queueKeys)
            {
                queueKeys.Enqueue(new ActionBatch(actions));
            }

            if (debugMode)
            {
                if (bw == null)
                {
                    bw = new BackgroundWorker();
                    bw.WorkerReportsProgress = true;
                    bw.WorkerSupportsCancellation = true;
                    bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                }
                if (!bw.IsBusy)
                    bw.RunWorkerAsync();
            }
            else
            {
                try
                {
                    DoWork(null);
                    //同步操作，则回零位
                    //BatchReset();
                }
                catch (Exception ex)
                {
                    //出错了，尝试归零
                    log.Error("测试盒点击过程中出现异常。", ex);
                    throw;
                }
            }
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                log.Error("测试盒点击过程中出现异常。", e.Error);
                throw e.Error;
            }
            lock (queueKeys)
            {
                if (queueKeys.Count > 0)
                {
                    if (!bw.IsBusy)
                        bw.RunWorkerAsync();
                }
            }
        }
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            DoWork(bw);
        }
        private void DoWork(BackgroundWorker bw)
        {
            try
            {
                this.Moving = true;
                DoWork2(bw);
            }
            finally
            {
                this.Moving = false;
            }
        }
        private void DoWork2(BackgroundWorker bw)
        {
            while (true)
            {
                if (bw != null && bw.CancellationPending)
                    break;

                //如果队列为空，则等待一段时间
                int count = 0;
                lock (queueKeys)
                    count = queueKeys.Count;
                int startTime = Environment.TickCount;
                while (count < 1 && Environment.TickCount - startTime < this.DelayBeforeReset)
                {
                    Thread.Sleep(100);
                    lock (queueKeys)
                        count = queueKeys.Count;
                }

                ActionBatch action = null;
                lock (queueKeys)
                {
                    if (queueKeys.Count < 1)
                        break;
                    action = queueKeys.Dequeue();
                }
                if (action == null)
                    continue;

                this.BatchActions(action.Parts.ToArray());
                //Thread.Sleep(300);
                if (this.IsReset)
                {
                    //若有安全高度，先到安全高度再出场归位
                    BatchReset();
                }
                else if (!this.IContinue)
                {
                    if (this.QuickPointZ != null)
                        this.BatchActions(new BatchMoveZ((double)this.QuickPointZ));
                    else
                        this.BatchActions(new BatchMoveSafeZ());
                }
            }
        }

        #endregion
    }
}
