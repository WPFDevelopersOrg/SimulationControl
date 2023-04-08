using System;
using System.Windows.Media;

namespace WpfUtil
{
    /// <summary>
    /// 表示只包含一个绘图上下文的图层
    /// </summary>
    public class SingleLayer : Layer
    {
        public SingleLayer()
        {
            // 添加可视对象
            _dv = new DrawingVisual();
            AddVisualChild(_dv);
            AddLogicalChild(_dv);
            // 初始化绘图环境
            _dc = _dv.RenderOpen();
            _dc.Close();
            // 添加事件
            IsEnabledChanged += (_, _) => UpdateLayer();
        }

        #region 重写

        protected override int VisualChildrenCount => 1;

        protected override Visual GetVisualChild(int index) => _dv;

        public override void UpdateLayer()
        {
            _dc = _dv.RenderOpen();
            if (IsEnabled) OnUpdate();
            _dc.Close();
        }

        public override void Clear()
        {
            _dv.RenderOpen().Close();
        }

        public override void AddVisual(VisualBase visual)
        {
            throw new Exception($"“{nameof(SingleLayer)}”不支持添加可视对象");
        }

        public override void DeleteVisual(VisualBase visual)
        {
            throw new Exception($"“{nameof(SingleLayer)}”不支持删除可视对象");
        }

        #endregion

        protected virtual void OnUpdate() { }

        #region 字段

        /// <summary>默认可视对象</summary>
        private readonly DrawingVisual _dv;
        /// <summary>默认绘图环境</summary>
        protected DrawingContext _dc;

        #endregion
    }
}