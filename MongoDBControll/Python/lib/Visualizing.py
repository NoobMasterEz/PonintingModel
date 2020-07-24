import numpy as np  # numpy array 
import matplotlib.pyplot as plt  
from pprint import pprint

class PlotAstropy(object):


    def  __init__(self,json,):
        self.__object = json

    @staticmethod
    def PlotVisual(symbol1="+",symbol2="*",**data):
        """
        axi x:  input x  type only list 2 dimention 
        axi y : input y  type only list 2 dimention 
        symbol
        """
        plt.plot(data["x1"],abs(data["y1"]),symbol1,color=data["color1"])
        plt.plot(data["x2"],abs(data["y2"]),symbol2,color=data["color2"])
        plt.xlabel(data["labelX"])
        plt.ylabel(data["labelX"])
        plt.title(data["title"]) 
        plt.show()


    @property
    def showDetail(self):
        pprint(self.__object)
    
    @property
    def GetObject(self):
        return self.__object