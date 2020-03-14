using Loxodon.Framework.Binding;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Localizations;
using Loxodon.Framework.Services;
using Loxodon.Framework.Views;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace LinkGame
{
    public class UILauncher : MonoBehaviour
    {
        private ApplicationContext context;

        private void Awake()
        {
            //获取全局的窗口管理器
            GlobalWindowManager windowManager = FindObjectOfType<GlobalWindowManager>();
            if (windowManager == null)
                throw new NotFoundException("Not found the GlobalWindowManager.");
            //获得应用上下文
            context = Context.GetApplicationContext();
            //获取上下文中的服务容器
            IServiceContainer container = context.GetContainer();
            //初始化数据绑定服务，这是一组服务，通过ServiceBundle来初始化并注册到服务容器中
            BindingServiceBundle bundle = new BindingServiceBundle(context.GetContainer());
            bundle.Start();
            //初始化UI定位器
            container.Register<IUIViewLocator>(new DefaultUIViewLocator());
            //获取当前语言信息
            CultureInfo cultureInfo = Locale.GetCultureInfo();
            //获取当前本地化服务
            var localization = Localization.Current;
            //设置语言
            localization.CultureInfo = cultureInfo;
            localization.AddDataProvider(new DefaultDataProvider("LocalizationExamples", new XmlDocumentParser()));

            container.Register<Localization>(localization);
        }
        // Start is called before the first frame update
        IEnumerator Start()
        {
            WindowContainer winContainer = WindowContainer.Create("MAIN");
            yield return null;

            IUIViewLocator locator = context.GetService<IUIViewLocator>();
            StartWindow window = locator.LoadWindow<StartWindow>(winContainer, "UI/StartWindow");
            window.Create();
            window.Show();

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
