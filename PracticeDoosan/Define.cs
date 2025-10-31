using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PracticeDoosan
{
    public class Define
    {
        public enum ROBOT_MODE
        {
            ROBOT_MODE_MANUAL,
            ROBOT_MODE_AUTONOMOUS,
            ROBOT_MODE_RECOVERY,
            ROBOT_MODE_BACKDRIVE,
            ROBOT_MODE_MEASURE,
            ROBOT_MODE_INITIALIZE,
            ROBOT_MODE_LAST
        };

        public enum ROBOT_CONTROL
        {
            CONTROL_INIT_CONFIG,
            CONTROL_ENABLE_OPERATION,
            CONTROL_RESET_SAFET_STOP,
            CONTROL_RESET_SAFE_STOP = CONTROL_RESET_SAFET_STOP,
            CONTROL_RESET_SAFET_OFF,
            CONTROL_RESET_SAFE_OFF = CONTROL_RESET_SAFET_OFF,
            CONTROL_SERVO_ON = CONTROL_RESET_SAFET_OFF,
            CONTROL_RECOVERY_SAFE_STOP,
            CONTROL_RECOVERY_SAFE_OFF,
            CONTROL_RECOVERY_BACKDRIVE,
            CONTROL_RESET_RECOVERY,
            CONTROL_LAST
        };

        public enum ROBOT_STATE
        {
            STATE_INITIALIZING,
            STATE_STANDBY,
            STATE_MOVING,
            STATE_SAFE_OFF,
            STATE_TEACHING,
            STATE_SAFE_STOP,
            STATE_EMERGENCY_STOP,
            STATE_HOMMING,
            STATE_RECOVERY,
            STATE_SAFE_STOP2,
            STATE_SAFE_OFF2,
            STATE_RESERVED1,
            STATE_RESERVED2,
            STATE_RESERVED3,
            STATE_RESERVED4,
            STATE_NOT_READY = 15,
            STATE_LAST,
        }

        public enum MANAGE_ACCESS_CONTROL
        {
            MANAGE_ACCESS_CONTROL_FORCE_REQUEST,
            MANAGE_ACCESS_CONTROL_REQUEST,
            MANAGE_ACCESS_CONTROL_RESPONSE_YES,
            MANAGE_ACCESS_CONTROL_RESPONSE_NO,
        };

        public enum MONITORING_ACCESS_CONTROL
        {
            MONITORING_ACCESS_CONTROL_REQUEST,
            MONITORING_ACCESS_CONTROL_DENY,
            MONITORING_ACCESS_CONTROL_GRANT,
            MONITORING_ACCESS_CONTROL_LOSS,
            MONITORING_ACCESS_CONTROL_LAST
        }

        public enum STOP_TYPE
        {
            STOP_TYPE_QUICK_STO = 0,
            STOP_TYPE_QUICK,
            STOP_TYPE_SLOW,
            STOP_TYPE_HOLD,
            STOP_TYPE_EMERGENCY = STOP_TYPE_HOLD,
        };

        public enum JOG_AXIS
        {
            J1 = 0,
            J2,
            J3,
            J4,
            J5,
            J6
        };

        public enum COORDINATE_SYSTEM
        {
            COORDINATE_SYSTEM_BASE = 0,
            COORDINATE_SYSTEM_TOOL,
            COORDINATE_SYSTEM_WORLD,
            COORDINATE_SYSTEM_USER_MIN = 101,
            COORDINATE_SYSTEM_USER_MAX = 200,
        };

        public enum MOVE_REFERENCE
        {
            MOVE_REFERENCE_BASE = 0,
            MOVE_REFERENCE_TOOL,
            MOVE_REFERENCE_WORLD,
            MOVE_REFERENCE_USER_MIN = 101,
            MOVE_REFERENCE_USER_MAX = 200,
        };

        // J 값 다 6으로 하드코딩
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ROBOT_MONITORING_JOINT
        {
            public fixed float _fActualPos[6];
            public fixed float _fActualAbs[6];
            public fixed float _fActualVel[6];
            public fixed float _fActualErr[6];
            public fixed float _fTargetPos[6];
            public fixed float _fTargetVel[6];
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ROBOT_MONITORING_TASK
        {
            // 2D 배열 [2][6]은 1D 배열로 Flatten 시켜야 함 (2 * 6 = 12)
            public fixed float _fActualPos[12];
            public fixed float _fActualVel[6];
            public fixed float _fActualErr[6];
            public fixed float _fTargetPos[6];
            public fixed float _fTargetVel[6];
            public byte _iSolutionSpace;

            // 3x3 배열 → 1D 배열로 Flatten
            public fixed float _fRotationMatrix[9];
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ROBOT_MONITORING_TORQUE
        {
            public fixed float _fDynamicTor[6];
            public fixed float _fActualJTS[6];
            public fixed float _fActualEJT[6];
            public fixed float _fActualETT[6];
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ROBOT_MONITORING_STATE
        {
            public byte _iActualMode;   // position control: 0, torque control: 1
            public byte _iActualSpace;  // joint space: 1, task space: 2
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct MONITORING_CONTROL
        {
            public ROBOT_MONITORING_STATE _tState;
            public ROBOT_MONITORING_JOINT _tJoint;
            public ROBOT_MONITORING_TASK _tTask;
            public ROBOT_MONITORING_TORQUE _tTorque;
        }

        public enum MOVE_HOME
        {
            MOVE_HOME_MECHANIC,
            MOVE_HOME_USER
        };

        public enum MOVE_MODE
        {
            MOVE_MODE_ABSOLUTE = 0,
            MOVE_MODE_RELATIVE,
        };

        public enum BLENDING_SPEED_TYPE
        {
            BLENDING_SPEED_TYPE_DUPLICATE = 0,
            BLENDING_SPEED_TYPE_OVERRIDE,
        };

        private const int MAX_STRING_SIZE = 256;

        [StructLayout(LayoutKind.Sequential, Pack =1)]
        public struct LOG_ALARM
        {
            public byte _iLevel;              // unsigned char = byte (1 byte)
            public byte _iGroup;              // unsigned char = byte (1 byte)
            public uint _iIndex;              // unsigned int = uint (4 bytes)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 768)]
            public byte[] _szParam;    // char[3][256]
        }


    }
}
