using System;
using System.Collections.Generic;
using System.Text;
using Mobot;
using TestDevice;

namespace Motor
{
    /// <summary>
    /// 暂停一段时间（毫秒）。
    /// </summary>
    public class BatchReset : IMotorAction
    {
        public BatchReset() { }
        public IMotorCommand ToMotorCommand(IControllerSettings settings)
        {
            return new MotorDriver.CommandReset();
        }
    }
    /// <summary>
    /// 暂停一段时间（毫秒）。
    /// </summary>
    public class BatchSleep : IMotorAction
    {
        public int MS { get; set; }
        public BatchSleep(int ms)
        {
            this.MS = ms;
        }
        public IMotorCommand ToMotorCommand(IControllerSettings settings)
        {
            return new MotorDriver.CommandSleep(this.MS);
        }
    }
    /// <summary>
    /// 移动X、Y轴
    /// </summary>
    public class BatchMoveXY : IMotorAction
    {
        public double X { get; set; }
        public double Y { get; set; }
        public BatchMoveXY(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        public IMotorCommand ToMotorCommand(IControllerSettings settings)
        {
            //int xTick = settings.MM2Ticks(this.X, Axis.X);
            //int yTick = settings.MM2Ticks(this.Y, Axis.Y);
            return new MotorDriver.CommandMoveXY((int)X, (int)Y);
        }
    }
    /// <summary>
    /// 移动Z轴
    /// </summary>
    public class BatchMoveZ : IMotorAction
    {
        public double Z { get; set; }
        public int Trigger { get; set; }
        public BatchMoveZ(double z)
        {
            this.Z = z;
        }
        public IMotorCommand ToMotorCommand(IControllerSettings settings)
        {
            //int zTick = settings.MM2Ticks(this.Z, Axis.Z);
            var temp = new MotorDriver.CommandMoveZ((int)Z);
            temp.Trigger = Trigger;
            return temp;
        }
    }

    /// <summary>
    /// 移动Z轴到上方近零高度
    /// </summary>
    public class BatchMoveSafeZ : IMotorAction
    {
        public BatchMoveSafeZ() { }
        public IMotorCommand ToMotorCommand(IControllerSettings settings)
        {
            return new MotorDriver.CommandMoveZ(50);
        }
    }

    public class BatchMove : IMotorAction
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public BatchMove(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        public IMotorCommand ToMotorCommand(IControllerSettings settings)
        {
            //int xTick = settings.MM2Ticks(this.X, Axis.X);
            //int yTick = settings.MM2Ticks(this.Y, Axis.Y);
            //int zTick = settings.MM2Ticks(this.Z, Axis.Z);
            return new MotorDriver.CommandMove((int)X, (int)Y, (int)Z);
        }
    }
    /// <summary>
    /// 临时改变电机速度。
    /// </summary>
    public class BatchMotorSpeed : IMotorAction
    {
        public Point3 Speed { get; set; }
        public BatchMotorSpeed(Point3 speed)
        {
            this.Speed = speed;
        }
        public IMotorCommand ToMotorCommand(IControllerSettings settings)
        {
            return new MotorDriver.CommandMotorSpeed(this.Speed);
        }
    }
    /// <summary>
    /// 恢复电机速度。
    /// </summary>
    public class BatchMotorSpeedReset : IMotorAction
    {
        public IMotorCommand ToMotorCommand(IControllerSettings settings)
        {
            return new MotorDriver.CommandMotorSpeedReset();
        }
    }
    public class BatchTimes : IMotorAction
    {
        public int Times { get; set; }
        public BatchTimes(int times)
        {
            this.Times = times;
        }
        public IMotorCommand ToMotorCommand(IControllerSettings settings)
        {
            return new MotorDriver.CommandTimes(this.Times);
        }
    }

    /// <summary>
    /// 画圆孤
    /// </summary>
    public class BatchDrawCircle : IMotorAction
    {
        public CircleType Type { get; private set; }
        public CircleLength Length { get; private set; }
        public int Radiu { get; private set; }
        public double X { get; set; }
        public double Y { get; set; }
        public BatchDrawCircle(CircleType type, CircleLength length, int radiu)
        {
            this.Type = type;
            this.Length = length;
            this.Radiu = radiu;
        }
        public IMotorCommand ToMotorCommand(IControllerSettings settings)
        {
            int xTick1 = settings.MM2Ticks(this.X, Axis.X);
            int xTick2 = settings.MM2Ticks(this.Y, Axis.X);
            return new MotorDriver.CommandDrawCircle(Type, Length, Math.Abs(xTick1 - xTick2));
        }
    }
}
