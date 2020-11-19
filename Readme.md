## 说明

一个插件式开发脚手架


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