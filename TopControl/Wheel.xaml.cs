using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TopControl
{
    public partial class Wheel : UserControl
    {
        public Wheel()
        {
            InitializeComponent();
            Loaded += Wheel_Loaded;
        }

        #region 属性

        public int MinValue { get; set; } = -720;

        public int MaxValue { get; set; } = 720;

        public int Value { get; set; } = -720;

        #endregion

        #region 控件事件

        private void Wheel_Loaded(object sender, RoutedEventArgs e)
        {
            InitLayer();
        }

        private void WheelArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                BeginDrag();
            }
        }

        private void WheelArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragMode)
            {
                Drag();
            }
        }

        private void WheelArea_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && _dragMode)
            {
                EndDrag();
            }
        }

        private void WheelArea_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_dragMode) return;

            int offset = e.Delta / 120;
            if (offset < 0 && Value > MinValue)
            {
                Value += offset;
                UpdateProgress();
                _wheelLayer.AngleOffset -= offset * _wheelSpeed;
                _wheelLayer.UpdateLayer();
            }
            else if (offset > 0 && Value < MaxValue)
            {
                Value += offset;
                UpdateProgress();
                _wheelLayer.AngleOffset -= offset * _wheelSpeed;
                _wheelLayer.UpdateLayer();
            }
        }

        #endregion

        #region 鼠标操作

        private void BeginDrag()
        {
            _dragMode = true;
            WheelArea.CaptureMouse();

            _dragStart = Mouse.GetPosition(WheelArea);
            _angleStart = _wheelLayer.AngleOffset;

            _valueStart = Value;
            _offsetDown = Value - MinValue;
            _offsetUp = Value - MaxValue;
        }

        private void Drag()
        {
            double offset_y = Mouse.GetPosition(WheelArea).Y - _dragStart.Y;
            if (offset_y < _offsetUp) offset_y = _offsetUp;
            else if (offset_y > _offsetDown) offset_y = _offsetDown;

            double offset_angle = offset_y * _wheelSpeed;
            Value = _valueStart - (int)offset_y;
            _wheelLayer.AngleOffset = _angleStart + offset_angle;
            UpdateProgress();
            _wheelLayer.UpdateLayer();
        }

        private void EndDrag()
        {
            _dragMode = false;
            WheelArea.ReleaseMouseCapture();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化图层
        /// </summary>
        private void InitLayer()
        {
            _wheelLayer.Width = LayerBox.ActualWidth;
            _wheelLayer.Height = LayerBox.ActualHeight;
            LayerBox.Children.Add(_wheelLayer);
            _wheelLayer.Init();
            _wheelLayer.UpdateLayer();
        }

        /// <summary>
        /// 更新进度
        /// </summary>
        private void UpdateProgress()
        {
            Grid_Value.Height = (double)(Value - MinValue) / (MaxValue - MinValue) * 180;
        }

        #endregion

        #region 字段

        private readonly WheelLayer _wheelLayer = new WheelLayer();

        private Point _dragStart = new Point();
        private double _angleStart = 0;
        private int _valueStart = 0;
        private bool _dragMode = false;

        /// <summary>滚轮速度</summary>
        private readonly double _wheelSpeed = 0.7;

        /// <summary>最大向上偏移</summary>
        private double _offsetUp;
        /// <summary>最大向下偏移</summary>
        private double _offsetDown;

        #endregion
    }
}