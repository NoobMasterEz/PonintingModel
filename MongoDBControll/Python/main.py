import json
import argparse
import numpy as np 
from pprint import pprint
from lib import Visualizing as vis 


def Getxy(obj):
    x=list()
    y=list()
    for i in range(int(obj.GetObject["numplate"])):
        x.append(list(obj.GetObject["data"])[i][0])
        y.append(list(obj.GetObject["data"])[i][1])
    return np.array(x),np.array(y)

def main():
    file1 =open("/mnt/c/Users/specter/Desktop/Mongo/MongoDBControll/Json/db.json") 
    file2 =open("/mnt/c/Users/specter/Desktop/Mongo/MongoDBControll/Json/plant.json")
    obj1=vis.PlotAstropy(json.load(file1))
    obj2=vis.PlotAstropy(json.load(file2))
    #obj.showDetail
    x1,y1= Getxy(obj1)
    x2,y2= Getxy(obj2)
    obj1.PlotVisual(x1=x1,y1=y1,x2=x2,y2=y2,color1="red",color2="green",symbol="+",symbol1="*",labelX="RA",labelY="Dec",title="Ra:Dec")




if __name__ == "__main__":
            main()
            