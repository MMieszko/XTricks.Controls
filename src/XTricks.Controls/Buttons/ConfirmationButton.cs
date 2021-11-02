using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XTricks.Controls.Extensions;

namespace XTricks.Controls.Buttons
{
    public class ConfirmationButton : Button
    {
        private string _initialText;
        private bool _approvalRequested;
        private Color _initialColor;
        private CancellationTokenSource _cancellationTokenSource;

        private bool HasConfirmationColor => this.ConfirmationBackgroundColor != default;

        public ICommand ConfirmCommand
        {
            get => (ICommand)GetValue(ConfirmCommandProperty);
            set => SetValue(ConfirmCommandProperty, value);
        }

        public object ConfirmCommandParameter
        {
            get => GetValue(ConfirmCommandParameterProperty);
            set => SetValue(ConfirmCommandParameterProperty, value);
        }

        public string ConfirmationText
        {
            get => (string)GetValue(ConfirmationTextProperty);
            set => SetValue(ConfirmationTextProperty, value);
        }

        public int TimeoutSeconds
        {
            get => (int)GetValue(TimeoutSecondsProperty);
            set => SetValue(TimeoutSecondsProperty, value);
        }

        public Color ConfirmationBackgroundColor
        {
            get => (Color)GetValue(ConfirmationBackgroundColorProperty);
            set => SetValue(ConfirmationBackgroundColorProperty, value);
        }

        public ConfirmationButton()
        {
            base.Pressed += OnPressed;
        }

        private void OnPressed(object sender, EventArgs e)
        {
            if (!_approvalRequested)
            {
                RequestApprove();
            }
            else
            {
                Approve();
            }
        }

        private void RequestApprove()
        {
            this._cancellationTokenSource = new CancellationTokenSource();
            this._initialColor = this.BackgroundColor;
            this._initialText = base.Text;
            this._approvalRequested = true;

            base.SetValue(TextProperty, this.ConfirmationText);
            this.Animate();
            this.RunTimeout(_cancellationTokenSource.Token);

        }

        private void Animate()
        {
            if (!HasConfirmationColor)
                return;

            this.ColorTo(this.BackgroundColor, this.ConfirmationBackgroundColor, (color) => SetValue(BackgroundColorProperty, color), (uint)TimeSpan.FromSeconds(0.5).TotalMilliseconds);
        }

        private void Approve()
        {
            this.ConfirmCommand?.Execute(this.ConfirmCommandParameter);

            this._cancellationTokenSource.Cancel();
        }

        private async void RunTimeout(CancellationToken token)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(TimeoutSeconds), token);
            }
            catch (TaskCanceledException)
            {

            }
            finally
            {
                Device.BeginInvokeOnMainThread(() => base.SetValue(TextProperty, _initialText));

                if (HasConfirmationColor)
                    Device.BeginInvokeOnMainThread(() => this.ColorTo(this.ConfirmationBackgroundColor, this._initialColor, (color) => SetValue(BackgroundColorProperty, color), (uint)TimeSpan.FromSeconds(0.5).TotalMilliseconds));

                _approvalRequested = false;
            }
        }


        #region - Bindable Properties -
        public static readonly BindableProperty ConfirmationTextProperty =
            BindableProperty.Create(nameof(ConfirmationText), typeof(string), typeof(ConfirmationButton), string.Empty);

        public static readonly BindableProperty TimeoutSecondsProperty =
            BindableProperty.Create(nameof(ConfirmationText), typeof(int), typeof(ConfirmationButton), 3);

        public static readonly BindableProperty ConfirmationBackgroundColorProperty =
            BindableProperty.Create(nameof(ConfirmationBackgroundColor), typeof(Color), typeof(ConfirmationButton));

        public static readonly BindableProperty ConfirmCommandProperty =
            BindableProperty.Create(nameof(ConfirmCommand), typeof(ICommand), typeof(ConfirmationButton));

        public static readonly BindableProperty ConfirmCommandParameterProperty =
            BindableProperty.Create(nameof(ConfirmCommandParameter), typeof(object), typeof(ConfirmationButton));
        #endregion
    }
}