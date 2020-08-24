# sample_project
# 取样项目上位机触摸屏界面

#### 环境要求
.net4.5.2

#### 运行环境配置
安装NDP452-KB2901907-x86-x64-AllOS-ENU\.exe , 添加路由
+ 一路按照默认选项遇next就点, 遇accept就同意
+ 安装TwinCat后随便新建一个项目, 扫描到倍福PLC然后登录一次
     
#### 文件结构介绍
winform触摸屏上位机控制
+ DevComponents\.DotNetBar\.Charts\.dll   
+ DevComponents\.DotNetBar\.Keyboard\.dll
+ DevComponents\.DotNetBar\.Keyboard\.xml
+ DevComponents\.DotNetBar\.Schedule\.dll
+ DevComponents\.DotNetBar\.Schedule\.xml
+ DevComponents\.DotNetBar\.SuperGrid\.dll
+ DevComponents\.DotNetBar\.SuperGrid\.xml
+ DevComponents\.DotNetBar2\.dll
+ DevComponents\.DotNetBar2\.xml
+ DevComponents\.Instrumentation\.dll
+ DevComponents\.Instrumentation\.xml
+ gc\.log
+ names\.txt
+ PLC\.config
+ TwAdsClass\.dll
+ TwAdsClass\.pdb
+ TwinCAT\.Ads\.dll
+ 触摸屏界面\.exe
+ 触摸屏界面\.exe\.config
+ 触摸屏界面\.pdb
+ 触摸屏界面\.vshost\.exe
+ 触摸屏界面\.vshost\.exe\.config
+ 触摸屏界面\.vshost\.exe\.manifest

unity动画
+ ConsoleTest\.exe
+ gc\.log
+ ModelShow\.exe
+ ModelShow_Data
+ QY43_Mini\.exe
+ QY43_Mini_Data

#### 重要文件配置
在触摸屏界面\.exe所在文件夹中找到PLC.config，以文本格式打开（用记事本打开）
```
<PLC>
	<!-- 远程, 修改此行为6段PLC地址 -->
	<ip address="5.78.120.12.1.1"/>

```
