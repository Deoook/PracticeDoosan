using System.Runtime.InteropServices;
using static PracticeDoosan.Define;


namespace PracticeDoosan.Model
{
    public class DoosanRobot
    {
        private IntPtr _rbtCtrl;

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr _CreateRobotControl();

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern bool _open_connection(IntPtr rbtCtrl, string ipAddr, uint port);

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool _close_connection(IntPtr rbtCtrl);

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool _set_robot_control(IntPtr rbtCtrl, ROBOT_CONTROL eControl);

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int _servo_off(IntPtr rbtCtrl, STOP_TYPE eStopType);

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool _set_robot_mode(IntPtr rbtCtrl, ROBOT_MODE eMode);

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern ROBOT_MODE _get_robot_mode(IntPtr rbtCtrl);

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool _ManageAccessControl(IntPtr rbtCtrl, MANAGE_ACCESS_CONTROL eAccessControl = MANAGE_ACCESS_CONTROL.MANAGE_ACCESS_CONTROL_REQUEST);

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool _Jog(IntPtr rbtCtrl, JOG_AXIS eJogAxis, MOVE_REFERENCE eMoveReference, float fVelocity);

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void _set_on_tp_initializing_completed(IntPtr rbtCtrl, TOnTpInitializingCompletedCB pCallbackFunc);

        public delegate void TOnTpInitializingCompletedCB();

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void _set_on_monitoring_access_control(IntPtr rbtCtrl, TOnMonitoringAccessControlCB pCallbackFunc);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void TOnMonitoringAccessControlCB(MONITORING_ACCESS_CONTROL data);

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void _set_on_monitoring_data(IntPtr rbtCtrl, TOnMonitoringDataCB pCallbackFunc);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        unsafe public delegate void TOnMonitoringDataCB(ref MONITORING_CONTROL data);

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void _set_on_log_alarm(IntPtr rbtCtrl, TOnLogAlarmCB pCallbackFunc);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void TOnLogAlarmCB(ref LOG_ALARM data);

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void _set_on_monitoring_state(IntPtr rbtCtrl, TOnMonitoringStateCB pCallbackFunc);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void TOnMonitoringStateCB(ROBOT_STATE data);

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool _amovej(IntPtr pCtrl, float[] fTargetPos, float fTargetVel, float fTargetAcc, float fTargetTime = 0, MOVE_MODE eMoveMode = MOVE_MODE.MOVE_MODE_ABSOLUTE, BLENDING_SPEED_TYPE eBlendingType = BLENDING_SPEED_TYPE.BLENDING_SPEED_TYPE_DUPLICATE);

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool _stop(IntPtr pCtrl, STOP_TYPE eStopType = STOP_TYPE.STOP_TYPE_QUICK);

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool _move_home(IntPtr rbtCtrl, MOVE_HOME eMode = MOVE_HOME.MOVE_HOME_USER, byte bRun = 1);

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern  bool _set_user_home(IntPtr rbtCtrl);

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool _movej(IntPtr rbtCtrl, float[] fTargetPos, float fTargetVel, float fTargetAcc, float fTargetTime = 0f, MOVE_MODE eMoveMode = MOVE_MODE.MOVE_MODE_ABSOLUTE, float fBlendingRadius = 0f, BLENDING_SPEED_TYPE eBlendingType = BLENDING_SPEED_TYPE.BLENDING_SPEED_TYPE_DUPLICATE);

        [DllImport("dll\\DRFLWin64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void _set_on_tp_log(IntPtr pCtrl, TOnTpLogCB pCallbackFunc);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        unsafe public delegate void TOnTpLogCB(byte* msg);

        public DoosanRobot()
        {
            _rbtCtrl = _CreateRobotControl();
            if (_rbtCtrl == IntPtr.Zero)
            {
                throw new Exception("Failed to create robot controller");
            }
        }
        public bool OpenConnection(string ip = "192.168.137.100", uint port = 12345)
        {
            return _open_connection(_rbtCtrl, ip, port);
        }

        public bool CloseConnection()
        {
            return _close_connection(_rbtCtrl);
        }

        public bool SetRobotControl(ROBOT_CONTROL eControl)
        {
            return _set_robot_control(_rbtCtrl, eControl);
        }

        public int ServoOff(STOP_TYPE eStopType)
        {
            return _servo_off(_rbtCtrl, eStopType);
        }

        public ROBOT_MODE GetRobotMode()
        {
            return _get_robot_mode(_rbtCtrl);
        }

        public bool SetRobotMode(ROBOT_MODE eMode)
        {
            return _set_robot_mode(_rbtCtrl, eMode);
        }

        public bool ManageAccessControl(MANAGE_ACCESS_CONTROL eAccessControl = MANAGE_ACCESS_CONTROL.MANAGE_ACCESS_CONTROL_REQUEST)
        {
            return _ManageAccessControl(_rbtCtrl, eAccessControl);
        }

        public bool Jog(JOG_AXIS eJogAxis, MOVE_REFERENCE eMoveReference, float fVelocity)
        {
             return _Jog(_rbtCtrl, eJogAxis, eMoveReference, fVelocity);
        }

        public void SetOnMonitoringData(TOnMonitoringDataCB pCallbackFunc)
        {
            _set_on_monitoring_data(_rbtCtrl, pCallbackFunc);
        }

        public void SetOnLogAlarm(TOnLogAlarmCB pCallbackFunc)
        {
            _set_on_log_alarm(_rbtCtrl, pCallbackFunc);
        }

        public bool MoveHome(MOVE_HOME eMode = MOVE_HOME.MOVE_HOME_USER, byte bRun = 1)
        {
            return _move_home(_rbtCtrl, eMode, bRun);
        }

        public bool SetUserHome()
        {
            return _set_user_home(_rbtCtrl);
        }

        public bool movej(float[] fTargetPos, float fTargetVel, float fTargetAcc, float fTargetTime = 0, MOVE_MODE eMoveMode = MOVE_MODE.MOVE_MODE_ABSOLUTE, float fBlendingRadius = 0, BLENDING_SPEED_TYPE eBlendingType = BLENDING_SPEED_TYPE.BLENDING_SPEED_TYPE_DUPLICATE)
        {
            return _movej(_rbtCtrl, fTargetPos, fTargetVel, fTargetAcc, fTargetTime, eMoveMode, fBlendingRadius, eBlendingType);
        }

        public bool amovej(float[] fTargetPos, float fTargetVel, float fTargetAcc, float fTargetTime = 0, MOVE_MODE eMoveMode = MOVE_MODE.MOVE_MODE_ABSOLUTE, BLENDING_SPEED_TYPE eBlendingType = BLENDING_SPEED_TYPE.BLENDING_SPEED_TYPE_DUPLICATE)
        {
            return _amovej(_rbtCtrl, fTargetPos, fTargetVel, fTargetAcc, fTargetTime, eMoveMode, eBlendingType);
        }

        public bool stop(STOP_TYPE eStopType = STOP_TYPE.STOP_TYPE_QUICK)
        {
            return _stop(_rbtCtrl, eStopType);
        }

        public void set_on_tp_log(TOnTpLogCB callback)
        {
            _set_on_tp_log(_rbtCtrl, callback);
        }

        public void SetOnTpInitializingCompleted(TOnTpInitializingCompletedCB callback)
        {
            _set_on_tp_initializing_completed(_rbtCtrl, callback);
        }

        public void SetOnMonitoringAccessControl(TOnMonitoringAccessControlCB callback)
        {
            _set_on_monitoring_access_control(_rbtCtrl, callback);
        }

        public void SetOnMonitoringState(TOnMonitoringStateCB callback)
        {
            _set_on_monitoring_state(_rbtCtrl, callback);   
        }
    }
}

