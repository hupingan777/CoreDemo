﻿using System;

namespace ServicesCollection.Tool.DanLi
{
    //单例模式(Singleton)也叫单态模式，是设计模式中最为简单的一种模式，甚至有些模式大师都不称其为模式，称其为一种实现技巧，因为设计模式讲究对象之间的关系的抽象，而单例模式只有自己一个对象，也因此有些设计大师并把把其称为设计模式之一。

    /*
    咱们周边和很有意义的单例应用场景：
        1. Windows的Task Manager（任务管理器）就是很典型的单例模式（这个很熟悉吧），想想看，是不是呢，你能打开两个windows task manager吗？ 不信你自己试试看哦~ 
        2. windows的Recycle Bin（回收站）也是典型的单例应用。在整个系统运行过程中，回收站一直维护着仅有的一个实例。
        3. 网站的计数器，一般也是采用单例模式实现，否则难以同步。
        4. 应用程序的日志应用，一般都何用单例模式实现，这一般是由于共享的日志文件一直处于打开状态，因为只能有一个实例去操作，否则内容不好追加。
        5. Web应用的配置对象的读取，一般也应用单例模式，这个是由于配置文件是共享的资源。
        6. 数据库连接池的设计一般也是采用单例模式，因为数据库连接是一种数据库资源。数据库软件系统中使用数据库连接池，主要是节省打开或者关闭数据库连接所引起的效率损耗，这种效率上的损耗还是非常昂贵的，因为何用单例模式来维护，就可以大大降低这种损耗。
        7. 多线程的线程池的设计一般也是采用单例模式，这是由于线程池要方便对池中的线程进行控制。
        8. 操作系统的文件系统，也是大的单例模式实现的具体例子，一个操作系统只能有一个文件系统。
        9. HttpApplication 也是单位例的典型应用。熟悉ASP.Net(IIS)的整个请求生命周期的人应该知道HttpApplication也是单例模式，所有的HttpModule都共享一个HttpApplication实例.

    总结以上，不难看出：
　　单例模式应用的场景一般发现在以下条件下：
　　（1）资源共享的情况下，避免由于资源操作时导致的性能或损耗等。如上述中的日志文件，应用配置。
　　（2）控制资源的情况下，方便资源之间的互相通信。如线程池等。 
    */

    /*
    //使用方式：
    public class TestSingleton : Singleton<TestSingleton>
    {
        // 将构造函数私有化，防止外部通过new创建
        private TestSingleton() { }

        //other code ...
    }

    //获取该单例对象
    var testSingleton=TestSingleton.Instance;
    */

    /// <summary>
    /// 单例类基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> where T : class
    {
        class Nested
        {
            // 创建模板类实例，参数2设为true表示支持私有构造函数
            internal static readonly T Instance = Activator.CreateInstance(typeof(T), true) as T;
        }

        //private static T instance = null;

        /// <summary>
        /// 单例对象
        /// </summary>
        public static T Instance { get { return Nested.Instance; } }
    }
}
