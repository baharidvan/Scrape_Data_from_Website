# Scrape Data from Website
## Project Summary and How to Solve

In this project, it is desired to develop a C# console application that extracting data from a website. 


## Steps of Developing Application

## ●Extracting the Data
● First of all we need to take data from the website succesfully. To do that, it is planned to use Html Agility Pack which is Html Parser written in C#. The package is added to the console application.<br />
● Using only Html Agility pack couldnt be sufficient to extract the data from website. Response Html data is not suitable for parsing due to permission issues. Than Selenium package is also added. It is a package that automates browsers. To extract the data from the website, we didn't send request the address of the website directly, First open the website with new Google Chrome application using Selenium and get the document from there and load with Html Agility pack. <br />
● Also it is also sleeping commands and proxy edits for not take possible ip bans.<br />

## ●Parsing the Data
● We need to get the title and price of the products on homepage. It can be obtained the all titles from the homepage but prices should be taken from details pages of every product.<br />
● It is selected nodes from the document with taking the Xpath values of <a> tag. <br />
● There are 56 products on homepage. But some of them are ads, we dont want to take them. To do that it is controlled the title values of every <a> tag, if it has no title, it means that it is not the product we desired it is an ad. <br />
● To get the price values, first it is taken address of the every product `homeProduct.Url = mainUrl + node.Attributes["href"].Value;`<br />
● GetPrice() method is defined. For every product we get HtmlDocument with same approach used for the Homepage. Price of the product taken from details page using Xpath `//*[@class=\"classifiedInfo \"]/h3/text()` and added to products list. `homeProduct.Price = GetPrice(homeProduct.Url); //Send the product Url, scrap the website and take the price value`<br />
## ●Complete the list and write it to console and txt file

<img src="https://user-images.githubusercontent.com/115007582/207147872-c6b56673-18a3-45f8-aa24-df8e8e909e64.PNG" width=70% height=70%>

<img src="https://user-images.githubusercontent.com/115007582/207155194-4fc617a6-5b42-4a13-ac82-c57cbc6a257a.PNG" width=70% height=70%>
