using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Items
{
    public enum OptionRun
    {
        /// <summary>
        /// 什么都没有
        /// </summary>
        [Option("{'InterfaceName':'None', " +
            "'Describe':'空接口', " +
            "'Paramters':{" +
            "}}")]
        None,
       
        [Option("{'InterfaceName':'Goto_Position_By_Finger', " +
           "'Describe':'方向轴移动', " +
           "'Paramters':{" +
           "'angle':'0', " +
           "}}")]
        DirMove,
        [Option("{'InterfaceName':'Goto_Position_By_Finger', " +
        "'Describe':'平A', " +
        "'Paramters':{" +
        "'depth':'0', " +
        "}}")]
        FlatA,
        [Option("{'InterfaceName':'CameraSet', " +
           "'Describe':'释放技能1', " +
           "'Paramters':{" +
           "'X':'0', " +
           "'Y':'0', " +
           "'depth':'0' " +
           "}}")]
        Skill1,
        [Option("{'InterfaceName':'CameraSet', " +
            "'Describe':'释放技能2', " +
            "'Paramters':{" +
            "'X':'0', " +
            "'Y':'0', " +
            "'depth':'0' " +
            "}}")]
        Skill2,
        [Option("{'InterfaceName':'CameraSet', " +
            "'Describe':'释放技能3', " +
            "'Paramters':{" +
            "'X':'0', " +
            "'Y':'0', " +
            "'depth':'0' " +
            "}}")]
        Skill3,
        [Option("{'InterfaceName':'CameraSet', " +
           "'Describe':'技能加点1', " +
           "'Paramters':{" +
           "'X':'0', " +
           "'Y':'0', " +
           "'depth':'0' " +
           "}}")]
        SkillPlus1,
        [Option("{'InterfaceName':'CameraSet', " +
           "'Describe':'技能加点2', " +
           "'Paramters':{" +
           "'X':'0', " +
           "'Y':'0', " +
           "'depth':'0' " +
           "}}")]

        SkillPlus2,
        [Option("{'InterfaceName':'CameraSet', " +
          "'Describe':'技能加点3', " +
          "'Paramters':{" +
          "'X':'0', " +
          "'Y':'0', " +
          "'depth':'0' " +
          "}}")]
        SkillPlus3,
        [Option("{'InterfaceName':'None', " +
             "'Describe':'停止', " +
             "'Paramters':{" +
             "}}")]
        StopMove,
        [Option("{'InterfaceName':'CameraSet', " +
          "'Describe':'点击装备', " +
          "'Paramters':{" +
          "'pointX':'0', " +
          "'pointY':'0', " +
          "'depth':'0' " +
          "}}")]
        ClickWeapons
    }
}
