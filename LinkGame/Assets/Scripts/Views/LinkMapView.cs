using Loxodon.Framework.Binding;
using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Binding.Contexts;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Interactivity;
using Loxodon.Framework.Services;
using Loxodon.Framework.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LinkGame
{
    public class LinkMapView : UIView
    {
        public Button LinkButton1_0;
        public Button LinkButton1_1;
        public Button LinkButton2_0;
        public Button LinkButton2_1;
        public Button LinkButton3_0;
        public Button LinkButton3_1;

        private IDialogService dialogService;

        protected override void Awake()
        {
            Debug.Log("protected override void Awake()");
            ApplicationContext context = Context.GetApplicationContext();
            //获得上下文中的服务容器
            IServiceContainer container = context.GetContainer();
            IDialogService dialogService = new DefaultDialogService();
            container.Register<IDialogService>(dialogService);
        }

        protected override void Start()
        {
            ApplicationContext context = Context.GetApplicationContext();
            dialogService = context.GetService<IDialogService>();

            LinkMapViewModel linkMapViewModel = new LinkMapViewModel(null);
            IBindingContext bindingContext = this.BindingContext();
            bindingContext.DataContext = linkMapViewModel;

            BindingSet<LinkMapView, LinkMapViewModel> bindingSet = this.CreateBindingSet<LinkMapView, LinkMapViewModel>();
            bindingSet.Bind(this.LinkButton1_0).For(v => v.onClick).To(vm => vm.LinkButtonClickCommand).CommandParameter(1);
            bindingSet.Bind(this.LinkButton1_1).For(v => v.onClick).To(vm => vm.LinkButtonClickCommand).CommandParameter(1);
            bindingSet.Bind(this.LinkButton2_0).For(v => v.onClick).To(vm => vm.LinkButtonClickCommand).CommandParameter(2);
            bindingSet.Bind(this.LinkButton2_1).For(v => v.onClick).To(vm => vm.LinkButtonClickCommand).CommandParameter(2);
            bindingSet.Bind(this.LinkButton3_0).For(v => v.onClick).To(vm => vm.LinkButtonClickCommand).CommandParameter(3);
            bindingSet.Bind(this.LinkButton3_1).For(v => v.onClick).To(vm => vm.LinkButtonClickCommand).CommandParameter(3);
            bindingSet.Bind().For(v => v.DeleteButtons).To(vm => vm.LinkButtonClickInteractionRequest);

            bindingSet.Build();
        }

        /// <summary>
        /// 消除对应的两个linkbutton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void DeleteButtons(object sender, InteractionEventArgs args)
        {
            int number = (int)args.Context;
            Debug.Log("消除number为：" + number);
            if (number == 1)
            {

                this.LinkButton1_0.gameObject.SetActive(false);
                this.LinkButton1_1.gameObject.SetActive(false);
            }
            else if (number == 2)
            {
                this.LinkButton2_0.gameObject.SetActive(false);
                this.LinkButton2_1.gameObject.SetActive(false);
            }
            else if (number == 3)
            {
                this.LinkButton3_0.gameObject.SetActive(false);
                this.LinkButton3_1.gameObject.SetActive(false);
            }
            if (IsSuccess())
            {
                this.dialogService.ShowDialog("提示", "恭喜通关！", "确定");
            }
        }

        private bool IsSuccess()
        {
            if (this.LinkButton1_0.gameObject.activeSelf)
            {
                return false;
            }
            if (this.LinkButton1_1.gameObject.activeSelf)
            {
                return false;
            }
            if (this.LinkButton2_0.gameObject.activeSelf)
            {
                return false;
            }
            if (this.LinkButton2_1.gameObject.activeSelf)
            {
                return false;
            }
            if (this.LinkButton3_0.gameObject.activeSelf)
            {
                return false;
            }
            if (this.LinkButton3_1.gameObject.activeSelf)
            {
                return false;
            }
            return true;
        }


    }

}
