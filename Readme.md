一个插件式开发脚手架

## 说明

为了易于维护起见，软件开发通常会采用分层式开发。但是单纯分层式架构+单体式架构，会将所有Domain下的代码分别混入各层。随着业务的增长，代码库势必会逐渐膨胀，使得代码越来越难以维护。

有时候微服务并不是一个好的选择，至少在调试上是这样。

对于单体式应用，最直接的解决办法当然是创建一个**核心**，负责导入基础服务；而对于每个领域下的模型，均以单独的一个项目或者多个项目开发(**插件式**)；最后将各个项目集成到**总承**上。

插件可以调用核心暴露出来的**核心服务**，插件与插件之间的通讯则采用类似于**中介者模式** 的方式进行。这样一来，就可以把各个领域的代码都分离到各自的代码库中，从而减少了项目整体复杂度。

不过，如果直接分离成多个项目插件包，则容易带来循环依赖问题(参考下图左侧示例，箭头代表依赖关系)：

| 单纯分层式架构 |  AsZero |
|---------------|-----------------|
| ![插件式开发1](docs/images/插件式架构-架构-插件式1.png?raw=true "插件式开发1") | ![插件式开发2](docs/images/插件式架构-架构-插件式2.png?raw=true "插件式开发1")  |

这个脚手架主要解决此问题。

## 如何使用


创建一个新的Git仓库，添加AsZero为远程仓库，然后创建一个本地分支追踪`AsZero/main`
```
git init
git remote add AsZero https://github.com/newbienewbie/AsZero
git fetch AsZero main
git checkout -b AsZero AsZero/main
```

然后创建一个本地分支
```
git checkout main
```

以插件式的型式开发

## 更新AsZero

```
git fetch AsZero
git checkout main
git merge AsZero/main
```