using SharpDX.DirectInput;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media;

namespace InputDisplay
{
    public class InputDisplayViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private Joystick joystick;

        #region INPC Properties
        private Brush northColor;
        private Brush northEastColor;
        private Brush northWestColor;
        private Brush eastColor;
        private Brush southColor;
        private Brush southEastColor;
        private Brush southWestColor;
        private Brush westColor;

        public Brush NorthColor
        {
            get => northColor;
            set
            {
                if (northColor != value)
                {
                    northColor = value;
                    OnPropertyChanged(nameof(NorthColor));
                }
            }
        }

        public Brush NorthEastColor
        {
            get => northEastColor;
            set
            {
                if (northEastColor != value)
                {
                    northEastColor = value;
                    OnPropertyChanged(nameof(NorthEastColor));
                }
            }
        }

        public Brush NorthWestColor
        {
            get => northWestColor;
            set
            {
                if (northWestColor != value)
                {
                    northWestColor = value;
                    OnPropertyChanged(nameof(NorthWestColor));
                }
            }
        }

        public Brush EastColor
        {
            get => eastColor;
            set
            {
                if (eastColor != value)
                {
                    eastColor = value;
                    OnPropertyChanged(nameof(EastColor));
                }
            }
        }

        public Brush SouthColor
        {
            get => southColor;
            set
            {
                if (southColor != value)
                {
                    southColor = value;
                    OnPropertyChanged(nameof(SouthColor));
                }
            }
        }

        public Brush SouthEastColor
        {
            get => southEastColor;
            set
            {
                if (southEastColor != value)
                {
                    southEastColor = value;
                    OnPropertyChanged(nameof(SouthEastColor));
                }
            }
        }

        public Brush SouthWestColor
        {
            get => southWestColor;
            set
            {
                if (southWestColor != value)
                {
                    southWestColor = value;
                    OnPropertyChanged(nameof(SouthWestColor));
                }
            }
        }

        public Brush WestColor
        {
            get => westColor;
            set
            {
                if (westColor != value)
                {
                    westColor = value;
                    OnPropertyChanged(nameof(WestColor));
                }
            }
        }
        #endregion

        public InputDisplayViewModel()
        {
            DirectInput directInput = new();
            var joystickGuid =
                directInput.GetDevices()
                    .Where(x => x.ProductName.Contains("PS4") && x.Type == DeviceType.FirstPerson)
                    .Select(y => y.InstanceGuid)
                    .Single();

            if (joystickGuid != Guid.Empty)
            {
                joystick = new Joystick(directInput, joystickGuid);
                joystick.Properties.BufferSize = 64;
                joystick.Acquire();
                Task.Run(() => { UpdateControllerInput(); });
            }
        }

        private void UpdateControllerInput()
        {
            while (true)
            {
                joystick.Poll();
                var datas = joystick.GetBufferedData();
                foreach (var state in datas)
                {
                    if (state.Offset == JoystickOffset.PointOfViewControllers0)
                    {
                        switch (state.Value)
                        {
                            case -1:
                                ResetAllDirectionColors();
                                break;
                            case 0:
                                NorthColor = Brushes.Red;
                                break;
                            case 4500:
                                NorthEastColor = Brushes.Red;
                                break;
                            case 9000:
                                EastColor = Brushes.Red;
                                break;
                            case 13500:
                                SouthEastColor = Brushes.Red;
                                break;
                            case 18000:
                                SouthColor = Brushes.Red;
                                break;
                            case 22500:
                                SouthWestColor = Brushes.Red;
                                break;
                            case 27000:
                                WestColor = Brushes.Red;
                                break;
                            case 31500:
                                northWestColor = Brushes.Red;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private void ResetAllDirectionColors()
        {
            NorthColor = Brushes.Transparent;
            NorthEastColor = Brushes.Transparent;
            EastColor = Brushes.Transparent;
            SouthEastColor = Brushes.Transparent;
            SouthColor = Brushes.Transparent;
            SouthWestColor = Brushes.Transparent;
            WestColor = Brushes.Transparent;
            NorthWestColor = Brushes.Transparent;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
