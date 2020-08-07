import re

class Auto:
    def common(self,control,name):
        with open('Main.Designer.cs','r',encoding='utf8')as f:
            code = f.read()  
        aim = re.findall('this\.'+control+'(.*?).Tag = ".+"', code)
        tag = {control+t:re.findall(control+t+'\.Tag = "(.*?);',
                                    code)[0].split('.')[1] for t in aim}
        if len(set(aim))!=len(tag):raise(Exception('功能重复'))
        for t in tag:
            for s in ',.)_ ;"':
                code = code.replace(t+s,name+'_'+tag[t]+s)
        with open('Main.Designer.cs','w',encoding='utf8')as f:
            f.write(code)
            
        with open('Main.cs','r',encoding='utf8')as f:
            code = f.read()
        for t in tag:
            for s in ',.)_ ;"':
                code = code.replace(t+s,name+'_'+tag[t]+s)
        with open('Main.cs','w',encoding='utf8')as f:
            f.write(code)  
            
    def txt(self):
        self.common('textBox','tbx')
            
    def btm(self):
        self.common('button','btm')
        
    def lb(self):
        self.common('label','lb')
        
    def A3A6(self):
        with open('Main.Designer.cs','r',encoding='utf8')as f:
            code = f.read()
        btn = re.findall('this\.(.*?)\.Tag = "HMI\.A\d_.*?\d;BOOL";',code)
        tag = {bt:re.findall(bt+'\.Tag = "HMI\.(.*?);BOOL',
                             code)[0] for bt in btn}
        for t in tag:
            for s in ',.)_ ;"':
                code = code.replace(t+s,'btn_'+tag[t]+s)
        with open('Main.Designer.cs','w',encoding='utf8')as f:
            f.write(code)

        with open('Main.cs','r',encoding='utf8')as f:
            code = f.read()
        for t in tag:
            for s in ',.)_ ;"':
                code = code.replace(t+s,'btn_'+tag[t]+s)
        with open('Main.cs','w',encoding='utf8')as f:
            f.write(code)
            
auto = Auto()
auto.txt()
auto.btm()
auto.lb()

