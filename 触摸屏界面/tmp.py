with open('Main.Designer.cs','r',encoding='utf8')as f:
    txt = f.read()
lines = txt.split('\n')
gp1 = [line for line in lines if 'groupPanel1' in line and '//' not in line]


gp2 = [line for line in lines if 'groupPanel2' in line and '//' not in line]


gp3 = [line for line in lines if 'groupPanel3' in line and '//' not in line]


gp1 = [i.replace('groupPanel1','') for i in gp1]
gp2 = [i.replace('groupPanel2','') for i in gp2]
gp3 = [i.replace('groupPanel3','') for i in gp3]

gp1.sort()
gp2.sort()
gp3.sort()

for i in gp1:
    if i not in gp3:
        print(i)