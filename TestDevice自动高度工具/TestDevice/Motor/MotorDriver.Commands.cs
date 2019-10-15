using System;
using System.Collections.Generic;
using System.Text;
using Mobot;
using TestDevice;

namespace Motor
{
    partial class MotorDriver
    {
        /// <summary>
        /// 重置。
        /// </summary>
        internal class CommandReset : IMotorCommand
        {
            public void Work(IMotorDriver driver)
            {
                ResetResponseMessage message = driver.Request(new ResetMessage()) as ResetResponseMessage;
                //Reset消息响应
                driver.Ticks = Point3.Empty;
            }
        }
        /// <summary>
        /// 暂停一段时间（毫秒）。
        /// </summary>
        internal class CommandSleep : IMotorCommand
        {
            public int MS { get; private set; }
            public CommandSleep(int ms)
            {
                this.MS = ms;
            }
            public void Work(IMotorDriver driver)
            {
                SleepMessage request = new SleepMessage(this.MS);
                SleepResponseMessage response = driver.Request(request) as SleepResponseMessage;
            }
        }
        /// <summary>
        /// 移动X、Y轴
        /// </summary>
        internal class CommandMoveXY : IMotorCommand
        {
            public int X { get; private set; }
            public int Y { get; private set; }
            public int Trigger { get; set; }
            public CommandMoveXY(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
            public void Work(IMotorDriver driver)
            {
                MoveMessage request = new MoveMessage(this.X, this.Y);
                request.Trigger = this.Trigger;
                MoveResponseMessage response = driver.Request(request) as MoveResponseMessage;

                //Move消息响应
                driver.Ticks = new Point3(this.X, this.Y, driver.Ticks.Z);
            }
        }

        

        /// <summary>
        /// 移动Z轴
        /// </summary>
        internal class CommandMoveZ : IMotorCommand
        {
            public int Z { get; private set; }
            public int Trigger { get; set; }
            public CommandMoveZ(int zTick)
            {
                this.Z = zTick;
            }
            public void Work(IMotorDriver driver)
            {
                MoveMessage request = new MoveMessage(this.Z);
                request.Trigger = this.Trigger;
                MoveResponseMessage response = driver.Request(request) as MoveResponseMessage;

                //Move消息响应
                driver.Ticks = new Point3(driver.Ticks.X, driver.Ticks.Y, this.Z);
            }
        }
        internal class CommandMove : IMotorCommand
        {
            public int X { get; private set; }
            public int Y { get; private set; }
            public int Z { get; private set; }
            public int Trigger { get; set; }
            public CommandMove(int xTick, int yTick, int zTick)
            {
                this.X = xTick;
                this.Y = yTick;
                this.Z = zTick;
            }
            public void Work(IMotorDriver driver)
            {
                MoveMessage request = new MoveMessage(this.X, this.Y, this.Z);
                request.Trigger = this.Trigger;
                MoveResponseMessage response = driver.Request(request) as MoveResponseMessage;

                //Move消息响应
                driver.Ticks = new Point3(this.X, this.Y, this.Z);
            }
        }
        /// <summary>
        /// 临时改变电机速度。
        /// </summary>
        internal class CommandMotorSpeed : IMotorCommand
        {
            public static Point3 PreSpeed = Point3.Empty;
            public Point3 Speed { get; private set; }
            public CommandMotorSpeed(Point3 speed)
            {
                this.Speed = speed;
            }
            public void Work(IMotorDriver driver)
            {
                CommandMotorSpeed.PreSpeed = driver.Speed;

                Point3 speed = this.Speed;
                if (speed != Point3.Empty)
                {
                    SetSpeedMessage request = new SetSpeedMessage(speed);
                    SetSpeedResponseMessage response = driver.Request(request) as SetSpeedResponseMessage;
                    //SetSpeed消息响应
                    driver.Speed = request.Speed;
                }
            }
        }
        /// <summary>
        /// 恢复电机速度。
        /// </summary>
        internal class CommandMotorSpeedReset : IMotorCommand
        {
            public void Work(IMotorDriver driver)
            {
                Point3 speed = CommandMotorSpeed.PreSpeed;
                if (speed != Point3.Empty)
                {
                    SetSpeedMessage request = new SetSpeedMessage(speed);
                    SetSpeedResponseMessage response = driver.Request(request) as SetSpeedResponseMessage;
                    //SetSpeed消息响应
                    driver.Speed = request.Speed;
                }
            }
        }

        internal class CommandMode : IMotorCommand
        {
            readonly BatchMode mode = BatchMode.Clear;
            public CommandMode(BatchMode mode)
            {
                this.mode = mode;
            }
            public void Work(IMotorDriver driver)
            {
                driver.SetBatchMode(this.mode);
            }
        }
        internal class CommandTimes : IMotorCommand
        {
            readonly int Times;
            public CommandTimes(int times)
            {
                this.Times = times;
            }
            public void Work(IMotorDriver driver)
            {
                driver.SetBatchTimes(this.Times);
            }
        }

        /// <summary>
        /// 画圆孤
        /// </summary>
        internal class CommandDrawCircle : IMotorCommand
        {
            public CircleType Type { get; private set; }
            public CircleLength Length { get; private set; }
            public int Radiu { get; private set; }
            public CommandDrawCircle(CircleType type, CircleLength length, int radiu)
            {
                this.Type = type;
                this.Length = length;
                this.Radiu = radiu;
            }
            public void Work(IMotorDriver driver)
            {
                DrawCircleMessage request = new DrawCircleMessage((int)Type, (int)Length, Radiu);
                DrawCircleResponseMessage response = driver.Request(request) as DrawCircleResponseMessage;
            }
        }
    }
}
