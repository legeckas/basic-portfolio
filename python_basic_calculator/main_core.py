import sys
from PyQt5 import QtWidgets
from main_window_ui import Ui_MainWindow


class ApplicationWindow(QtWidgets.QMainWindow):

    def __init__(self):
        super().__init__()
        self.ui = Ui_MainWindow()
        self.ui.setupUi(self)


class Calculator:

    def __init__(self):
        self.app = QtWidgets.QApplication(sys.argv)
        self.app_window = ApplicationWindow()
        self.app_window.show()

        self.button_assignment()

        self.numOne = None
        self.numTwo = None

        self.operation_id = None

        self.to_clear_boxes = False
        self.equaled = False

        sys.exit(self.app.exec_())

    def button_assignment(self):
        for child in self.app_window.ui.buttonFrame.children():
            if "number" in child.objectName() or "comma" in child.objectName():
                child.clicked.connect(self.character_entry)

        for child in self.app_window.ui.buttonFrame.children():
            if "ion" in child.objectName():
                child.clicked.connect(self.operation_setter)

        self.app_window.ui.deleteLastButton.clicked.connect(self.delete_last_character)
        self.app_window.ui.negativePositiveButton.clicked.connect(self.positive_negative_setter)
        self.app_window.ui.clearAllButton.clicked.connect(self.clear_all)
        self.app_window.ui.equalsButton.clicked.connect(self.equals_operation)

    def character_entry(self):
        sender = self.app_window.ui.buttonFrame.sender()

        if self.to_clear_boxes:
            self.app_window.ui.digitLabel.setText("0")
            self.clear_some()

        if len(self.app_window.ui.digitLabel.text()) == 16 and "." not in self.app_window.ui.digitLabel.text():
            return
        elif len(self.app_window.ui.digitLabel.text()) == 17 and "." in self.app_window.ui.digitLabel.text():
            return

        if self.app_window.ui.digitLabel.text() == "0" and sender.objectName() == "commaButton":
            self.app_window.ui.digitLabel.setText("0.")
            return
        elif self.app_window.ui.digitLabel.text() == "0":
            self.app_window.ui.digitLabel.clear()
            self.app_window.ui.digitLabel.setText(self.app_window.ui.digitLabel.text() + sender.text())
        elif sender.objectName() == "commaButton" and "." in self.app_window.ui.digitLabel.text():
            return
        else:
            self.app_window.ui.digitLabel.setText(self.app_window.ui.digitLabel.text() + sender.text())

    def operation_setter(self):
        sender = self.app_window.ui.buttonFrame.sender()

        if self.app_window.ui.digitLabel.text() == "Cannot divide by zero":
            return

        if self.app_window.ui.digitLabel.text()[-1] == ".":
            self.app_window.ui.digitLabel.setText(self.app_window.ui.digitLabel.text()[:-1])

        try:
            self.numOne = int(self.app_window.ui.digitLabel.text())
        except ValueError:
            self.numOne = float(self.app_window.ui.digitLabel.text())

        self.operation_id = sender.objectName()[:-6]
        self.to_clear_boxes = True

        self.app_window.ui.historyLabel.setText(self.app_window.ui.digitLabel.text() + " " + sender.text())

    def equals_operation(self):

        result = None

        if self.app_window.ui.digitLabel.text() == "Cannot divide by zero" or self.operation_id is None:
            return
        elif self.equaled:
            try:
                self.numOne = int(self.app_window.ui.digitLabel.text())
            except ValueError:
                self.numOne = float(self.app_window.ui.digitLabel.text())
            self.history_label_setter()
        else:
            try:
                self.numTwo = int(self.app_window.ui.digitLabel.text())
            except ValueError:
                self.numTwo = float(self.app_window.ui.digitLabel.text())
            self.to_clear_boxes = True
            self.history_label_setter()

        if self.operation_id == "division":
            if self.numTwo == 0:
                self.set_digit_label_font_size()
                self.app_window.ui.digitLabel.setText("Cannot divide by zero")
                self.to_clear_boxes = True
            else:
                result = str(round(self.numOne / self.numTwo, 15))
        elif self.operation_id == "multiplication":
            result = str(self.numOne * self.numTwo)
        elif self.operation_id == "deduction":
            result = str(self.numOne - self.numTwo)
        elif self.operation_id == "addition":
            result = str(self.numOne + self.numTwo)

        self.determine_font_size(result)
        self.app_window.ui.digitLabel.setText(result)

    def delete_last_character(self):
        if self.app_window.ui.digitLabel.text() == "Cannot divide by zero":
            self.app_window.ui.digitLabel.setText("0")
        elif self.equaled:
            self.clear_some()

        self.app_window.ui.digitLabel.setText(self.app_window.ui.digitLabel.text()[:-1])

        if self.app_window.ui.digitLabel.text() == "" or self.app_window.ui.digitLabel.text() == "-":
            self.app_window.ui.digitLabel.setText("0")

    def clear_all(self):

        self.app_window.ui.digitLabel.setText("0")
        self.numOne = None
        self.numTwo = None
        self.operation_id = None
        self.app_window.ui.historyLabel.clear()
        self.clear_some()

    def clear_some(self):
        if self.equaled:
            self.app_window.ui.historyLabel.clear()

        self.to_clear_boxes = False
        self.equaled = False

        self.set_digit_label_font_size()

    def positive_negative_setter(self):
        if self.app_window.ui.digitLabel.text()[0] == "-":
            self.app_window.ui.digitLabel.setText(self.app_window.ui.digitLabel.text()[1:])
        elif self.app_window.ui.digitLabel.text() == "0":
            return
        else:
            self.app_window.ui.digitLabel.setText("-" + self.app_window.ui.digitLabel.text())

    def history_label_setter(self):
        operation_symbols = {
            "division": "÷",
            "multiplication": "x",
            "deduction": "-",
            "addition": "+"
        }

        self.app_window.ui.historyLabel.setText("{numOne} {operation} {numTwo} =".format(
            numOne=self.numOne,
            numTwo=self.numTwo,
            operation=operation_symbols[self.operation_id]
        ))

    def set_digit_label_font_size(self, font_size=20):
        font = self.app_window.ui.digitLabel.font()
        font.setPointSize(font_size)
        self.app_window.ui.digitLabel.setFont(font)

    def determine_font_size(self, label_string):
        length = len(label_string)

        if length > 28:
            self.set_digit_label_font_size(11)
        elif length > 25:
            self.set_digit_label_font_size(13)
        elif length > 22:
            self.set_digit_label_font_size(14)
        elif length > 18:
            self.set_digit_label_font_size(16)
        elif length > 16:
            self.set_digit_label_font_size(18)
        else:
            self.set_digit_label_font_size(20)


if __name__ == "__main__":
    Calculator()
