# -*- coding: utf-8 -*-

# Form implementation generated from reading ui file 'price_getter_ui.ui'
#
# Created by: PyQt5 UI code generator 5.13.2
#
# WARNING! All changes made in this file will be lost!


from PyQt5 import QtCore, QtGui, QtWidgets


class Ui_MainWindow(object):
    def setupUi(self, MainWindow):
        MainWindow.setObjectName("MainWindow")
        MainWindow.resize(245, 70)
        self.centralwidget = QtWidgets.QWidget(MainWindow)
        self.centralwidget.setObjectName("centralwidget")
        self.gridLayout = QtWidgets.QGridLayout(self.centralwidget)
        self.gridLayout.setObjectName("gridLayout")
        self.keywordComboBox = QtWidgets.QComboBox(self.centralwidget)
        self.keywordComboBox.setMinimumSize(QtCore.QSize(125, 0))
        self.keywordComboBox.setObjectName("keywordComboBox")
        self.gridLayout.addWidget(self.keywordComboBox, 0, 0, 1, 1)
        self.showButton = QtWidgets.QPushButton(self.centralwidget)
        self.showButton.setObjectName("showButton")
        self.gridLayout.addWidget(self.showButton, 0, 1, 1, 1)
        self.updateButton = QtWidgets.QPushButton(self.centralwidget)
        self.updateButton.setMinimumSize(QtCore.QSize(225, 0))
        self.updateButton.setObjectName("updateButton")
        self.gridLayout.addWidget(self.updateButton, 1, 0, 1, 1)
        MainWindow.setCentralWidget(self.centralwidget)

        self.retranslateUi(MainWindow)
        QtCore.QMetaObject.connectSlotsByName(MainWindow)

    def retranslateUi(self, MainWindow):
        _translate = QtCore.QCoreApplication.translate
        MainWindow.setWindowTitle(_translate("MainWindow", "MainWindow"))
        self.showButton.setText(_translate("MainWindow", "Show"))
        self.updateButton.setText(_translate("MainWindow", "Update"))
