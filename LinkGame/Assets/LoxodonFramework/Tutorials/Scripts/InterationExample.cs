/*
 * MIT License
 *
 * Copyright (c) 2018 Clark Yang
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in 
 * the Software without restriction, including without limitation the rights to 
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
 * of the Software, and to permit persons to whom the Software is furnished to do so, 
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all 
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
 * SOFTWARE.
 */

using System;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Loxodon.Framework.Contexts;
using Loxodon.Framework.Views;

using Loxodon.Framework.Localizations;
using Loxodon.Framework.Binding;
using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.ViewModels;
using Loxodon.Framework.Interactivity;
using Loxodon.Framework.Commands;
using Loxodon.Framework.Services;
using Loxodon.Framework.Views.InteractionActions;

namespace Loxodon.Framework.Tutorials
{
    public class InterationViewModel : ViewModelBase
    {
        private InteractionRequest<DialogNotification> alertDialogRequest;
        private InteractionRequest<ToastNotification> toastRequest;
        private InteractionRequest<VisibilityNotification> loadingRequest;

        private SimpleCommand openAlertDialog;
        private SimpleCommand showToast;
        private SimpleCommand showLoading;
        private SimpleCommand hideLoading;

        public InterationViewModel()
        {
            //创建一个交互请求，这个交互请求的作用就是向控制层(InteractionExample)发送一个打开对话窗的通知
            this.alertDialogRequest = new InteractionRequest<DialogNotification>(this);
            this.toastRequest = new InteractionRequest<ToastNotification>(this);
            this.loadingRequest = new InteractionRequest<VisibilityNotification>();

            //创建一个打响应按钮事件的命令
            this.openAlertDialog = new SimpleCommand(() =>
            {
                //设置命令的Enable为false，通过数据绑定解耦，间接将视图层按钮设置为不可点击状态
                this.openAlertDialog.Enabled = false;
                //创建一个对话框通知
                DialogNotification notification = new DialogNotification("Interation Example", "This is a dialog test.", "Yes", "No", true);
                //创建一个回调函数，此回调函数会在AlertDialog对话框关闭时调用
                Action<DialogNotification> callback = n =>
                {
                    //设置命令的Enable为true，通过绑定会自动恢复按钮的点击状态
                    this.openAlertDialog.Enabled = true;

                    if (n.DialogResult == AlertDialog.BUTTON_POSITIVE)
                    {
                        Debug.LogFormat("Click: Yes");
                    }
                    else if (n.DialogResult == AlertDialog.BUTTON_NEGATIVE)
                    {
                        Debug.LogFormat("Click: No");
                    }
                };
                //交互请求向View层OnOpenAlert函数发送通知
                this.alertDialogRequest.Raise(notification, callback);
            });

            this.showToast = new SimpleCommand(() =>
            {
                ToastNotification notification = new ToastNotification("This is a toast test.", 2f);
                this.toastRequest.Raise(notification);
            });

            this.showLoading = new SimpleCommand(() =>
            {
                VisibilityNotification notification = new VisibilityNotification(true);
                this.loadingRequest.Raise(notification);
            });

            this.hideLoading = new SimpleCommand(() =>
            {
                VisibilityNotification notification = new VisibilityNotification(false);
                this.loadingRequest.Raise(notification);
            });

        }

        public IInteractionRequest AlertDialogRequest { get { return this.alertDialogRequest; } }
        public IInteractionRequest ToastRequest { get { return this.toastRequest; } }
        public IInteractionRequest LoadingRequest { get { return this.loadingRequest; } }

        public ICommand OpenAlertDialog { get { return this.openAlertDialog; } }
        public ICommand ShowToast { get { return this.showToast; } }
        public ICommand ShowLoading { get { return this.showLoading; } }
        public ICommand HideLoading { get { return this.hideLoading; } }
    }

    public class InterationExample : WindowView
    {
        public Button openAlert;
        public Button showToast;
        public Button showLoading;
        public Button hideLoading;

        private List<Loading> list = new List<Loading>();

        private LoadingInteractionAction loadingInteractionAction;
        private ToastInteractionAction toastInteractionAction;

        protected override void Awake()
        {
            ApplicationContext context = Context.GetApplicationContext();
            BindingServiceBundle bindingService = new BindingServiceBundle(context.GetContainer());
            bindingService.Start();

            /* Initialize the ui view locator and register UIViewLocator */
            IServiceContainer container = context.GetContainer();
            container.Register<IUIViewLocator>(new DefaultUIViewLocator());

            CultureInfo cultureInfo = Locale.GetCultureInfo();
            var localization = Localization.Current;
            localization.CultureInfo = cultureInfo;
            localization.AddDataProvider(new DefaultDataProvider("LocalizationTutorials", new XmlDocumentParser()));
            container.Register(localization);
        }

        protected override void Start()
        {
            this.loadingInteractionAction = new LoadingInteractionAction();
            this.toastInteractionAction = new ToastInteractionAction(this);

            InterationViewModel viewModel = new InterationViewModel();
            this.SetDataContext(viewModel);

            /* databinding */
            BindingSet<InterationExample, InterationViewModel> bindingSet = this.CreateBindingSet<InterationExample, InterationViewModel>();

            /* Bind the method "OnOpenAlert" to an interactive request */
            //绑定本视图的OnOpenAlert函数到视图模型的交互请求AlertDialogRequest，当交互请求触发时，自动调用OnOpenAlert函数
            bindingSet.Bind().For(v => v.OnOpenAlert).To(vm => vm.AlertDialogRequest);

            /* Bind the ToastInteractionAction to an interactive request */
            bindingSet.Bind().For(v => v.toastInteractionAction).To(vm => vm.ToastRequest);
            /* or bind the method "OnShowToast" to an interactive request */
            //bindingSet.Bind().For(v => v.OnShowToast).To(vm => vm.ToastRequest);

            /* Bind the LoadingInteractionAction to an interactive request */
            bindingSet.Bind().For(v => v.loadingInteractionAction).To(vm => vm.LoadingRequest);
            /* or bind the method "OnShowOrHideLoading" to an interactive request */
            //bindingSet.Bind().For(v => v.OnShowOrHideLoading).To(vm => vm.LoadingRequest);

            /* Binding command */
            bindingSet.Bind(this.openAlert).For(v => v.onClick).To(vm => vm.OpenAlertDialog);
            bindingSet.Bind(this.showToast).For(v => v.onClick).To(vm => vm.ShowToast);
            bindingSet.Bind(this.showLoading).For(v => v.onClick).To(vm => vm.ShowLoading);
            bindingSet.Bind(this.hideLoading).For(v => v.onClick).To(vm => vm.HideLoading);

            bindingSet.Build();
        }

        private void OnOpenAlert(object sender, InteractionEventArgs args)
        {
            DialogNotification notification = args.Context as DialogNotification;
            var callback = args.Callback;

            if (notification == null)
                return;

            AlertDialog.ShowMessage(notification.Message, notification.Title, notification.ConfirmButtonText, null, notification.CancelButtonText, notification.CanceledOnTouchOutside, (result) =>
              {
                  notification.DialogResult = result;

                  if (callback != null)
                      callback();
              });
        }

        private void OnShowToast(object sender, InteractionEventArgs args)
        {
            ToastNotification notification = args.Context as ToastNotification;
            if (notification == null)
                return;

            Toast.Show(this, notification.Message, notification.Duration);
        }

        private void OnShowOrHideLoading(object sender, InteractionEventArgs args)
        {
            VisibilityNotification notification = args.Context as VisibilityNotification;
            if (notification == null)
                return;

            if (notification.Visible)
            {
                this.list.Add(Loading.Show());
            }
            else
            {
                if (this.list.Count <= 0)
                    return;

                Loading loading = this.list[0];
                loading.Dispose();
                this.list.RemoveAt(0);
            }
        }
    }
}
