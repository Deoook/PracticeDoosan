using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PracticeDoosan.Model;
using PracticeDoosan.Service;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static PracticeDoosan.Define;
using static PracticeDoosan.Model.DoosanRobot;

namespace PracticeDoosan.ViewModel
{
    public partial class MainVM : ObservableObject
    {
        /**************************************************************************************************************/
        /*    선언부                                                                                                  */
        /**************************************************************************************************************/
        #region 선언부

        // 연결 IP
        [ObservableProperty]
        string _iP;

        // 축 선택 상태
        [ObservableProperty]
        private JOG_AXIS selectedJoint = JOG_AXIS.J1;

        // Jog 이동 속도
        [ObservableProperty]
        private float speedValue = 20f;

        // 화면 우측 상단에 뿌려주는 상태
        [ObservableProperty]
        private string _state = "STATE_DISCONNECT";

        [ObservableProperty]
        private double valueJ1;

        [ObservableProperty]
        private double valueJ2;

        [ObservableProperty]
        private double valueJ3;

        [ObservableProperty]
        private double valueJ4;

        [ObservableProperty]
        private double valueJ5;

        [ObservableProperty]
        private double valueJ6;

        // Jog 값 모니터링 델리게이트
        private TOnMonitoringDataCB _delegateMonitoring;

        // Alarm 모니터링 델리게이트
        private TOnLogAlarmCB _delegateAlarm;

        // Log 모니터링 델리게이트 -> 필요 없을듯?
        private TOnTpLogCB _delegateLog;

        // 연결 완료 모니터링 델리게이트
        private TOnTpInitializingCompletedCB _delegateInitializingCompleted;

        // 권한 모니터링 델리게이트
        private TOnMonitoringAccessControlCB _delegateMonitoringAccessControl;

        // 로봇 상태 모니터링 델리게이트
        private TOnMonitoringStateCB _delegateMonitoringState;

        [ObservableProperty]
        private ObservableCollection<float> pose1 = new ObservableCollection<float>
        {
            0, 0, 0, 0, 0, 0
        };

        [ObservableProperty]
        private ObservableCollection<float> pose2 = new ObservableCollection<float>
        {
            0, 0, 0, 0, 0, 0
        };

        [ObservableProperty]
        private ObservableCollection<float> pose3 = new ObservableCollection<float>
        {
            0, 0, 0, 0, 0, 0
        };

        // Model 인스턴스
        private DoosanRobot _doosanRobot;

        // Wait 창 인터페이스
        private readonly ILoadingService _loadingService;


        // 로봇 연결 상태 Bool
        [ObservableProperty]
        private bool _isConnected = false;

        // 서보 모터 상태 Bool
        [ObservableProperty]
        private bool _isServoOn = false;
        #endregion

        /**************************************************************************************************************/
        /*    생성자                                                                                                  */
        /**************************************************************************************************************/
        #region 생성자
        public MainVM(ILoadingService loadingService)
        {
            do
            {
                if (false == Initialize())
                    break;

                _loadingService = loadingService;

            } while (false);

        }

        unsafe public bool Initialize()
        {
            bool bReturn = false;
            do
            {
                _doosanRobot = new DoosanRobot();


                IP = "192.168.137.100";

                _delegateMonitoring = new TOnMonitoringDataCB(OnMonitoringDataCB);
                _delegateAlarm = new TOnLogAlarmCB(OnMonitoringAlarmCB);
                _delegateLog = new TOnTpLogCB(OnTpLog);
                _delegateInitializingCompleted = new TOnTpInitializingCompletedCB(OnTpInitializingCompleted);
                _delegateMonitoringAccessControl = new TOnMonitoringAccessControlCB(OnMonitoringAccessControl);
                _delegateMonitoringState = new TOnMonitoringStateCB(OnMonitoringState);

                bReturn = true;
            } while (false);

            return bReturn;
        }
        #endregion

        /**************************************************************************************************************/
        /*    FUNCTION                                                                                                */
        /**************************************************************************************************************/
        #region FUNCTION

        #region CONNECT / DISCONNECT
        [RelayCommand]
        public async Task Connection()
        {
            // 연결 상태 확인 후 Connect / Disconnect
            if (IsConnected == false)
            {
                IsConnected = await Connect(IP);
            }
            else
            {
                Disconnect();
            }
        }

        private async Task<bool> Connect(string IP)
        {
            bool success = false;

            try
            {
                // 로딩 창 띄우고
                _loadingService.Show("CONNECTING...");

                success = await Task.Run(() =>
                {
                    if (false == _doosanRobot.OpenConnection(IP))
                    {
                        Debug.WriteLine("OPEN FAIL");
                        return false;
                    }

                    // 연결 성공 했으면
                    _doosanRobot.SetOnMonitoringData(_delegateMonitoring);
                    _doosanRobot.SetOnLogAlarm(_delegateAlarm);
                    _doosanRobot.set_on_tp_log(_delegateLog);
                    _doosanRobot.SetOnTpInitializingCompleted(_delegateInitializingCompleted);
                    _doosanRobot.SetOnMonitoringAccessControl(_delegateMonitoringAccessControl);
                    _doosanRobot.SetOnMonitoringState(_delegateMonitoringState);

                    // AutoMode로 바꿔주기 -> AutoMode 아니면 권한을 못 가져오는거 같음..
                    SetRobotMode();
                    return true;
                });

                if (success)
                {
                    _loadingService.UpdateMessage("연결 완료!");
                    await Task.Delay(800);
                }
                else
                {
                    _loadingService.UpdateMessage("로봇 연결에 실패하였습니다.");
                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Connection Error: {ex.Message}");
            }
            finally
            {
                // 로딩 창 닫기
                _loadingService.Hide();
            }

            return success;
        }

        private void Disconnect()
        {
            _doosanRobot.CloseConnection();
            IsConnected = false;
            State = "STATE_DISCONNECT";
        }
        #endregion

        #region SERVO ON / OFF
        [RelayCommand]
        private void ServoControl()
        {
            // 서보 모터 상태에 따라 ON / OFF
            if (false == IsServoOn)
            {
                IsServoOn = ServoON();
            }
            else
            {
                ServoOFF();
            }
        }

        private bool ServoON()
        {
            bool bReturn = false;
            do
            {
                if (false == _doosanRobot.SetRobotControl(Define.ROBOT_CONTROL.CONTROL_SERVO_ON))
                    break;

                bReturn = true;
            } while (false);

            return bReturn;
        }

        private void ServoOFF()
        {
            do
            {
                _doosanRobot.ServoOff(Define.STOP_TYPE.STOP_TYPE_EMERGENCY);
                IsServoOn = false;
            } while (false);
        }
        #endregion

        #region ROBOT MODE GET / SET
        [RelayCommand]
        private void GetRobotMode()
        {
            int s = (int)_doosanRobot.GetRobotMode();
            Console.WriteLine(s);
        }

        [RelayCommand]
        private void SetRobotMode()
        {
            bool bReturn = false;
            do
            {
                if (false == _doosanRobot.SetRobotMode(Define.ROBOT_MODE.ROBOT_MODE_AUTONOMOUS))
                {
                    Debug.WriteLine("SET ROBOT MODE FAIL");
                    break;
                }

                bReturn = true;
            } while (false);
        }


        #endregion

        #region MOVE
        [RelayCommand]
        private void PlusJogStart()
        {
            do
            {
                bool bReturn = _doosanRobot.Jog(SelectedJoint, MOVE_REFERENCE.MOVE_REFERENCE_BASE, SpeedValue);
            } while (false);
        }

        [RelayCommand]
        private void MinusJogStart()
        {
            do
            {
                bool bReturn = _doosanRobot.Jog(SelectedJoint, MOVE_REFERENCE.MOVE_REFERENCE_BASE, -SpeedValue);
            } while (false);
        }

        [RelayCommand]
        private void JogStop()
        {
            do
            {
                bool bReturn = _doosanRobot.Jog(SelectedJoint, MOVE_REFERENCE.MOVE_REFERENCE_BASE, 0f);
            } while (false);
        }

        [RelayCommand]
        private void GoHome()
        {
            _doosanRobot.MoveHome();
        }

        [RelayCommand]
        private void SetHome()
        {
            _doosanRobot.SetUserHome();
        }

        [RelayCommand]
        private void GetPose(string sNum)
        {
            switch (sNum)
            {
                case "1":
                    Pose1[0] = (float)ValueJ1;
                    Pose1[1] = (float)ValueJ2;
                    Pose1[2] = (float)ValueJ3;
                    Pose1[3] = (float)ValueJ4;
                    Pose1[4] = (float)ValueJ5;
                    Pose1[5] = (float)ValueJ6;
                    break;
                case "2":
                    Pose2[0] = (float)ValueJ1;
                    Pose2[1] = (float)ValueJ2;
                    Pose2[2] = (float)ValueJ3;
                    Pose2[3] = (float)ValueJ4;
                    Pose2[4] = (float)ValueJ5;
                    Pose2[5] = (float)ValueJ6;
                    break;
                case "3":
                    Pose3[0] = (float)ValueJ1;
                    Pose3[1] = (float)ValueJ2;
                    Pose3[2] = (float)ValueJ3;
                    Pose3[3] = (float)ValueJ4;
                    Pose3[4] = (float)ValueJ5;
                    Pose3[5] = (float)ValueJ6;
                    break;
            }
        }

        [RelayCommand]
        private void GoPose(string sNum)
        {
            float jvel = 10;
            float jacc = 10;
            float[] pose = new float[6];

            // 서보 꺼져있으면 Return.
            if (false == IsServoOn)
                return;

            // 메뉴얼 모드여야 GO Pose 가능.
            _doosanRobot.SetRobotMode(Define.ROBOT_MODE.ROBOT_MODE_MANUAL);

            switch (sNum)
            {
                case "1":
                    pose = new float[6] { Pose1[0], Pose1[1], Pose1[2], Pose1[3], Pose1[4], Pose1[5] };
                    break;
                case "2":
                    pose = new float[6] { Pose2[0], Pose2[1], Pose2[2], Pose2[3], Pose2[4], Pose2[5] };
                    break;
                case "3":
                    pose = new float[6] { Pose3[0], Pose3[1], Pose3[2], Pose3[3], Pose3[4], Pose3[5] };
                    break;
            }

            _doosanRobot.amovej(pose, jvel, jacc);
        }

        [RelayCommand]
        private void Stop()
        {
            // 연결 되어있을 경우에만 Stop 가능하게
            if (IsConnected == true)
                _doosanRobot.stop();
        }
        #endregion

        #region DELEGATE
        unsafe private void OnMonitoringDataCB(ref MONITORING_CONTROL data)
        {
            ValueJ1 = Math.Round(data._tJoint._fActualPos[0], 2);
            ValueJ2 = Math.Round(data._tJoint._fActualPos[1], 2); ;
            ValueJ3 = Math.Round(data._tJoint._fActualPos[2], 2); ;
            ValueJ4 = Math.Round(data._tJoint._fActualPos[3], 2); ;
            ValueJ5 = Math.Round(data._tJoint._fActualPos[4], 2); ;
            ValueJ6 = Math.Round(data._tJoint._fActualPos[5], 2); ;
        }

        private void OnMonitoringAlarmCB(ref LOG_ALARM data)
        {
            const int MAX_STRING_SIZE = 256;

            MessageBox.Show($"Level: {data._iLevel}, Group: {data._iGroup}, Index: {data._iIndex} \n {((DefineError.ERROR_CODE)data._iIndex).ToString()}");
        }

        unsafe private void OnTpLog(byte* msg)
        {
            string log = Encoding.ASCII.GetString(msg, 256).TrimEnd('\0');
            Console.WriteLine($"[TP LOG] {log}");
        }

        private void OnTpInitializingCompleted()
        {
            // 제어권 가져오기
            ManageAccessControl();
        }

        [RelayCommand]
        private void ManageAccessControl()
        {
            // 이거 Time Out 필요할듯.
            bool bReturn = false;
            do
            {
                if (false == _doosanRobot.ManageAccessControl(MANAGE_ACCESS_CONTROL.MANAGE_ACCESS_CONTROL_FORCE_REQUEST))
                {
                    Debug.WriteLine("MANAGE ACCESS CONTROL FAIL");
                    break;
                }

                bReturn = true;
            } while (false);
        }

        private void OnMonitoringAccessControl(MONITORING_ACCESS_CONTROL data)
        {
            switch (data)
            {
                // 다른 요청 들어옴
                case MONITORING_ACCESS_CONTROL.MONITORING_ACCESS_CONTROL_REQUEST:
                    _doosanRobot.ManageAccessControl(MANAGE_ACCESS_CONTROL.MANAGE_ACCESS_CONTROL_RESPONSE_NO);
                    break;
                // 권한 받기 성공
                case MONITORING_ACCESS_CONTROL.MONITORING_ACCESS_CONTROL_GRANT:
                    break;
                // 권한 받기 실패 혹은 뺏겼을 경우 다시 권한 요청
                case MONITORING_ACCESS_CONTROL.MONITORING_ACCESS_CONTROL_DENY:
                case MONITORING_ACCESS_CONTROL.MONITORING_ACCESS_CONTROL_LOSS:
                    _doosanRobot
                    .ManageAccessControl(MANAGE_ACCESS_CONTROL.MANAGE_ACCESS_CONTROL_FORCE_REQUEST);
                    break;
            }
        }

        /// <summary>
        /// 로봇 상태 업데이트
        /// </summary>
        /// <param name="data"></param>
        private void OnMonitoringState(ROBOT_STATE data)
        {
            Console.WriteLine(data);
            State = ((ROBOT_STATE)data).ToString();
        }
        #endregion

        #endregion

    }
}
