import os
import re
import sys
import csv
import json
import urllib.request
import pandas as pd
import plotly.graph_objects as go
from time import gmtime, strftime, sleep
from price_getter_ui import Ui_MainWindow
from PyQt5 import QtWidgets


# Initializing UI
class ApplicationWindow(QtWidgets.QMainWindow):
    def __init__(self):
        super().__init__()
        self.ui = Ui_MainWindow()
        self.ui.setupUi(self)


# Class to manage the UI interaction with internal logic
class ApplicationManager:
    def __init__(self):
        self.app = QtWidgets.QApplication(sys.argv)
        self.window = ApplicationWindow()

        self.keywords = self.keyword_getter()

        self.method_assignment()

        self.window.show()
        sys.exit(self.app.exec_())

    # Assigns relations of methods to UI objects
    def method_assignment(self):
        self.window.ui.keywordComboBox.addItems(self.keywords)
        self.window.ui.showButton.clicked.connect(self.show_plot)
        self.window.ui.updateButton.clicked.connect(self.update_all)

    # Calls DataProcessor class to update data according to keywords
    def update_all(self):
        for keyword in self.keywords:
            DataProcessor(keyword).data_writer()

    # Calls Plotter class to plot the data for a selected keyword
    def show_plot(self):
        key_word = self.window.ui.keywordComboBox.currentText()
        Plotter(key_word)

    # Pulls keywords from a csv file
    def keyword_getter(self):
        dirpath = os.getcwd() + '\\keywords.csv'

        with open(dirpath, 'r', encoding='utf-8') as keywords:
            reader = csv.reader(keywords)
            data = list(reader)[0]

        return data


# Class to plot data for a given keyword
class Plotter:
    def __init__(self, key_word):
        self.key_word = key_word
        self.plot_data(self.pull_data(self.key_word))

    # Collects data from a csv file for a given key_word and returns it
    def pull_data(self, key_word):
        return pd.read_csv("lists/{key_word}.csv".format(key_word=self.key_word), index_col=0)

    # Plots the data received from 
    def plot_data(self, data):
        # Groups data by store name
        data_grouped = data.groupby("Store Name")

        fig = go.Figure()

        # Iterates through stores and products to add traces for each product
        for value in data['Store Name'].unique():
            for column in data_grouped.get_group(value).dropna(axis=1).drop("Store Name", axis=1).columns:
                fig.add_trace(go.Scatter(
                    x=data.index,
                    y=data[column],
                    mode='lines',
                    name=column,
                    text="{column} - {store}".format(column=column, store=value))
                )

        fig.update_layout(
            title="Price of {keyword}".format(keyword=self.key_word.lower()),
            xaxis=dict(
                title="Date",
                tickformat="%Y-%m-%d"
            ),
            yaxis_title="Price",
        )

        fig.show()


# Class to process the data from store getters into a pandas dataframe
class DataProcessor:
    def __init__(self, key_word):
        self.key_word = key_word
        self.current_date = strftime("%Y-%m-%d", gmtime())

    # Writes data received from store getters to a csv
    def data_writer(self):
        df_rimi = self.dataframe_creator(RimiReader(self.key_word))
        df_barb = self.dataframe_creator(BarboraReader(self.key_word))

        # Checks whether the file already exists and appends, if true
        try:
            df = pd.read_csv("lists/{key_word}.csv".format(key_word=self.key_word), index_col=0)
            df = df.append([df_rimi, df_barb], sort=True)

        # Otherwise merges data from two stores
        except FileNotFoundError:
            df = pd.merge(df_rimi, df_barb, on='Store Name', how='outer', sort=True)
            df = df.set_index(pd.Index([self.current_date, self.current_date], inplace=True))

        df.to_csv("lists/{key_word}.csv".format(key_word=self.key_word))

    # Converts store getter data into a pandas dataframe
    def dataframe_creator(self, data_reader):
        df = pd.DataFrame(data_reader.data, columns=data_reader.column_names, index=[self.current_date])
        df["Store Name"] = [data_reader.store_name]
        return df


# Class to scrape data from Rimi e-shop
class RimiReader:
    def __init__(self, key_word):
        self.store_name = "Rimi"
        self.key_word = key_word

        self.column_names = []
        self.data = []

        self.json_parser(self.json_getter(self.key_word))

    # Method that returns a json with product data
    def json_getter(self, key_word):
        key_word = urllib.parse.quote_plus(key_word)
        rimi_address = "https://www.rimi.lt/e-parduotuve/lt/paieska?pageSize=80&query={key_word}".format(
            key_word=key_word)
        rimi_source = urllib.request.urlopen(rimi_address).read().decode('raw_unicode_escape')
        rimi_json = rimi_source.split("dataLayer.push(")[2].split(");")[0]

        loaded_rimi_json = json.loads(rimi_json)

        return loaded_rimi_json

    # Method that parses data into lists
    def json_parser(self, json):
        products = json['ecommerce']['impressions']

        product_names = []
        product_prices = []

        for item in products:
            if self.key_word.lower() in item['name'].lower():
                product_names.append(item['name'])
                product_prices.append(item['price'])

        raw_data = list(zip(product_names, product_prices))

        value_list = []

        for item in raw_data:
            name, price = item

            value_list.append(price)
            self.column_names.append(name)

        self.data.append(value_list)


# Class to scrape data from Barbora e-shop
class BarboraReader:
    def __init__(self, key_word):
        self.store_name = "Barbora"
        self.key_word = key_word

        self.initial_list = []

        self.column_names = []
        self.data = [self.initial_list]

        self.page_lister()

    # Pulls data from specified number of pages in the e-shop
    def page_lister(self):
        for i in range(1, 3):
            try:
                self.data_maker(self.json_parser(self.data_getter(self.key_word, str(i))))
            except IndexError:
                continue
            sleep(10)

    # Method that returns a json with product data
    def data_getter(self, key_word, page_number):
        key_word = urllib.parse.quote_plus(key_word)
        barbora_address = "https://barbora.lt/paieska?q={key_word}&page={page_number}".format(key_word=key_word,
                                                                                              page_number=page_number)
        barbora_source = urllib.request.urlopen(barbora_address).read().decode('utf-8')
        barbora_json = barbora_source.split("var products = ")[1].split(";")[0]

        loaded_barbora_json = json.loads(barbora_json)

        return loaded_barbora_json

    # Method that parses data into lists
    def json_parser(self, json):

        product_names = []
        product_prices = []

        for item in json:
            if self.key_word.lower() in item['title'].lower():
                product_names.append(item['title'])
                product_prices.append(item['price'])

        raw_data = list(zip(product_names, product_prices))

        for item in raw_data:
            name, price = item

            self.initial_list.append(price)
            self.column_names.append(name)


if __name__ == "__main__":
    ApplicationManager()