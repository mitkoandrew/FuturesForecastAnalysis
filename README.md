# FuturesForecastAnalysis
Futures Forecast Analysis is a part of my bigger project **Futures Price Prediction**.
The entire project helps the client make a decision about operations in the stock market: Buy / Sell or do nothing with the selected stock or futures.
It includes three parts:

-Forecast algorythm

-Analysis part (FuturesForecastAnalysis)

-Trading system

The first two parts were implemented in .NET, the third part - in Python.

I realised to show this project just to demonstrate some of my skills in .NET (MVVM, creating UI in WPF, working with xml)

## How does it work:

Click the "Load Archive" button and choose the sample xml (trades_archive.xml). After loading, you will see the table and graph for the first code. You can change the 
code in the combobox on the left. 

Click the "Calculate Analysis" button. You will see changes in the table - columns "Result" and "Total" will be filled for several rows. 

Click the "Save Analysis" button to save the updated table to a new or existing xml file.
