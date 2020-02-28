import sys
from PyQt5 import QtWidgets
from PyQt5.QtWidgets import QMessageBox

from main_ui import Ui_MainWindow
import random


class ApplicationWindow(QtWidgets.QMainWindow):

    def __init__(self):
        super().__init__()
        self.ui = Ui_MainWindow()
        self.ui.setupUi(self)


class BattleshipsMain:

    def __init__(self):
        self.app = QtWidgets.QApplication(sys.argv)
        self.window = ApplicationWindow()
        self.window.ui.beginGameButton.hide()
        self.window.show()

        self.method_assignment()
        self.lobby()

        self.game_state = None
        self.x_coordinates = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J"]
        self.y_coordinates = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10"]

        self.possible_ship_sizes = [4, 3, 3, 2, 2, 2, 1, 1, 1, 1]
        self.existing_ships_list = []
        self.available_sizes = []

        self.enemy_ship_list = []
        self.all_possible_enemy_targets = []
        self.last_enemy_shot_coordinate = None
        self.last_enemy_shot_hit = False

        sys.exit(self.app.exec_())

    def method_assignment(self):
        for child in self.window.ui.ownButtonFrame.children():
            child.clicked.connect(self.ship_selection)

        for child in self.window.ui.enemyButtonFrame.children():
            child.clicked.connect(self.main_game_loop)

        self.window.ui.actionNew_Game.triggered.connect(self.new_game_setup)
        self.window.ui.beginGameButton.clicked.connect(self.main_game_initiate)

    def lobby(self):
        self.window.ui.ownButtonFrame.setEnabled(False)
        self.window.ui.enemyButtonFrame.setEnabled(False)

    def new_game_setup(self):
        self.window.ui.ownButtonFrame.setEnabled(True)
        self.window.ui.beginGameButton.show()
        # self.window.ui.beginGameButton.setEnabled(False)
        self.existing_ships_list = []
        self.available_sizes = []
        self.enemy_ship_list = []

        for button1 in self.window.ui.ownButtonFrame.children():
            for button2 in self.window.ui.enemyButtonFrame.children():
                button1.setText("")
                button1.setStyleSheet("QPushButton {background-color: ;}")
                button1.setEnabled(True)
                button1.setProperty("hasShip", False)
                button1.setProperty("shipSize", 0)
                button1.setProperty("shipTiles", [])
                button2.setText("")
                button2.setStyleSheet("QPushButton {background-color: ;}")
                button2.setProperty("hasShip", False)

    def main_game_initiate(self):
        self.window.ui.beginGameButton.hide()

        self.window.ui.ownButtonFrame.setDisabled(True)
        self.window.ui.enemyButtonFrame.setEnabled(True)

        self.enemy_fleet_generator()

        for x_coordinate in self.x_coordinates:
            for y_coordinate in self.y_coordinates:
                self.all_possible_enemy_targets.append(x_coordinate + y_coordinate)

    def main_game_loop(self):
        sender = self.window.ui.enemyButtonFrame.sender()
        hit = False

        if sender.property("hasShip"):
            sender.setText("x")
            hit = True
        else:
            sender.setText("•")
            hit = False

        sender.setEnabled(False)

        if hit:
            for ship in self.enemy_ship_list:
                if sender.objectName()[5:] in ship:
                    ship.remove(sender.objectName()[5:])

                    if ship == []:
                        self.enemy_ship_list.remove(ship)

        if self.enemy_ship_list == []:
            self.game_ends(True)

        self.enemy_makes_shot()

    def ship_selection(self):
        sender = self.window.ui.ownButtonFrame.sender()

        self.ship_size_available()

        if sender.styleSheet().__contains__("yellow"):
            sender.setStyleSheet("QPushButton {background-color: ;}")
            sender.setProperty("hasShip", False)
            self.toggle_diagonal_selection(True)

            for ship_coordinate in sender.property("shipTiles"):
                for tile in self.cross_coordinate_getter(ship_coordinate):
                    self.window.ui.ownButtonFrame.findChild(QtWidgets.QPushButton, "own" + tile).setEnabled(True)

            self.ship_size_coordinate_setter("decrease")
            self.ship_size_available()
        else:
            if self.ship_size_available():
                sender.setStyleSheet("QPushButton {background-color: #3B6D7F;}")
                sender.setProperty("hasShip", True)
                self.toggle_diagonal_selection(False)
                self.ship_size_coordinate_setter("increase")

        self.ship_size_available()

        for ship_list in self.existing_ships_list:
            for ship in ship_list:
                try:
                    if len(self.available_sizes) == 0:
                        for coordinate in self.cross_coordinate_getter(ship):
                            self.window.ui.ownButtonFrame.findChild(
                                QtWidgets.QPushButton, "own" + coordinate).setEnabled(False)
                    elif len(ship_list) >= max(self.available_sizes):
                        for coordinate in self.cross_coordinate_getter(ship):
                            self.window.ui.ownButtonFrame.findChild(
                                QtWidgets.QPushButton, "own" + coordinate).setEnabled(False)
                    elif len(ship_list) < max(self.available_sizes):
                        for coordinate in self.cross_coordinate_getter(ship):
                            self.window.ui.ownButtonFrame.findChild(
                                QtWidgets.QPushButton, "own" + coordinate).setEnabled(True)
                except ValueError:
                    continue

        for ship_list in self.existing_ships_list:
            for ship in ship_list:
                for coordinate in self.diagonal_coordinate_getter(ship):
                    self.window.ui.ownButtonFrame.findChild(QtWidgets.QPushButton, "own" + coordinate).setEnabled(
                        False)

        for ship_list in self.existing_ships_list:
            ship_list.sort()
            for ship in ship_list:
                self.window.ui.ownButtonFrame.findChild(QtWidgets.QPushButton, "own" + ship).setEnabled(True)

        if len(self.existing_ships_list) < 10:
            self.window.ui.beginGameButton.setEnabled(False)
        else:
            self.window.ui.beginGameButton.setEnabled(True)

    def toggle_diagonal_selection(self, can_select):
        diagonal_coordinate_list = self.diagonal_coordinate_getter()
        if can_select:
            for diagonal_coordinate in diagonal_coordinate_list:
                self.window.ui.ownButtonFrame.findChild(
                    QtWidgets.QPushButton, "own" + diagonal_coordinate).setEnabled(True)
        elif not can_select:
            for diagonal_coordinate in diagonal_coordinate_list:
                self.window.ui.ownButtonFrame.findChild(
                    QtWidgets.QPushButton, "own" + diagonal_coordinate).setEnabled(False)

    def ship_size_coordinate_setter(self, add_or_remove):
        sender = self.window.ui.ownButtonFrame.sender()

        adjacent_ships = []

        if add_or_remove == "increase":
            for coordinate in self.cross_coordinate_getter():
                if self.window.ui.ownButtonFrame.findChild(QtWidgets.QPushButton, "own" + coordinate).property(
                        "hasShip"):
                    ship_tile_list = self.window.ui.ownButtonFrame.findChild(
                        QtWidgets.QPushButton, "own" + coordinate).property("shipTiles")
                    ship_tile_list.sort()
                    adjacent_ships.append(coordinate)

                    self.existing_ships_list.remove(ship_tile_list)

                    ship_tile_list.append(sender.objectName()[3:])

                    ship_size = len(ship_tile_list)

                    if ship_tile_list not in self.existing_ships_list:
                        self.existing_ships_list.append(ship_tile_list)

                    for tile in ship_tile_list:
                        self.window.ui.ownButtonFrame.findChild(
                            QtWidgets.QPushButton, "own" + tile).setProperty("shipTiles", ship_tile_list)
                        self.window.ui.ownButtonFrame.findChild(
                            QtWidgets.QPushButton, "own" + tile).setProperty("shipSize", ship_size)

            if len(adjacent_ships) > 1:
                other_ships_list = []
                for ship in adjacent_ships:
                    for other_ship in self.window.ui.ownButtonFrame.findChild(
                            QtWidgets.QPushButton, "own" + ship).property("shipTiles"):
                        other_ships_list.append(other_ship)
                    self.existing_ships_list.remove(
                        self.window.ui.ownButtonFrame.findChild(
                            QtWidgets.QPushButton, "own" + ship).property("shipTiles"))

                other_ships_list = list(set(other_ships_list))

                for ship in other_ships_list:
                    self.window.ui.ownButtonFrame.findChild(
                        QtWidgets.QPushButton, "own" + ship).setProperty("shipTiles", other_ships_list)
                    self.window.ui.ownButtonFrame.findChild(
                        QtWidgets.QPushButton, "own" + ship).setProperty("shipSize", len(other_ships_list))

                self.existing_ships_list.append(other_ships_list)

            if sender.property("shipSize") == 0:
                sender_ship_tile_list = sender.property("shipTiles")
                sender_ship_tile_list.append(sender.objectName()[3:])
                sender.setProperty("shipTiles", sender_ship_tile_list)

                ship_size = len(sender_ship_tile_list)
                sender.setProperty("shipSize", ship_size)

                self.existing_ships_list.append(sender_ship_tile_list)

        elif add_or_remove == "decrease":
            middle_sorted_out = False
            for coordinate in self.cross_coordinate_getter():
                if self.window.ui.ownButtonFrame.findChild(QtWidgets.QPushButton, "own" + coordinate).property(
                        "hasShip"):
                    ship_tile_list = self.window.ui.ownButtonFrame.findChild(
                        QtWidgets.QPushButton, "own" + coordinate).property("shipTiles")
                    ship_tile_list.sort()

                    if sender.objectName()[3:] != ship_tile_list[0] and \
                            sender.objectName()[3:] != ship_tile_list[-1] and not middle_sorted_out:
                        self.existing_ships_list.remove(ship_tile_list)
                        index_to_remove = ship_tile_list.index(sender.objectName()[3:])
                        self.existing_ships_list.append(ship_tile_list[:index_to_remove])
                        self.existing_ships_list.append(ship_tile_list[index_to_remove + 1:])

                        for coordinate_left in ship_tile_list[:index_to_remove]:
                            self.window.ui.ownButtonFrame.findChild(
                                QtWidgets.QPushButton, "own" + coordinate_left).setProperty(
                                "shipTiles", ship_tile_list[:index_to_remove])
                            self.window.ui.ownButtonFrame.findChild(
                                QtWidgets.QPushButton, "own" + coordinate_left).setProperty(
                                "shipSize", len(ship_tile_list[:index_to_remove]))

                        for coordinate_left in ship_tile_list[index_to_remove + 1:]:
                            self.window.ui.ownButtonFrame.findChild(
                                QtWidgets.QPushButton, "own" + coordinate_left).setProperty(
                                "shipTiles", ship_tile_list[index_to_remove + 1:])
                            self.window.ui.ownButtonFrame.findChild(
                                QtWidgets.QPushButton, "own" + coordinate_left).setProperty(
                                "shipSize", len(ship_tile_list[index_to_remove + 1:]))
                        middle_sorted_out = True
                    elif (sender.objectName()[3:] == ship_tile_list[0]) or (
                            sender.objectName()[3:] == ship_tile_list[-1]) and not middle_sorted_out:
                        self.existing_ships_list.remove(ship_tile_list)
                        ship_tile_list.remove(sender.objectName()[3:])
                        ship_size = len(ship_tile_list)

                        for tile in ship_tile_list:
                            self.window.ui.ownButtonFrame.findChild(
                                QtWidgets.QPushButton, "own" + tile).setProperty("shipTiles", ship_tile_list)
                            self.window.ui.ownButtonFrame.findChild(
                                QtWidgets.QPushButton, "own" + tile).setProperty("shipSize", ship_size)
                        self.existing_ships_list.append(ship_tile_list)

                    sender.setProperty("shipTiles", [])

                    sender.setProperty("shipSize", 0)

            if sender.property("shipSize") == 1 and sender.property("shipTiles")[0] == sender.objectName()[3:]:
                sender_ship_tile_list = sender.property("shipTiles")

                self.existing_ships_list.remove(sender_ship_tile_list)

                sender_ship_tile_list.remove(sender.objectName()[3:])
                sender.setProperty("shipTiles", sender_ship_tile_list)

                ship_size = len(sender_ship_tile_list)
                sender.setProperty("shipSize", ship_size)

    def diagonal_coordinate_getter(self, ship_coordinate=None):
        if ship_coordinate is None:
            ship_coordinate = self.window.ui.centralwidget.sender().objectName()[3:]

        diagonal_x_coordinates = []
        diagonal_y_coordinates = []
        combined_diagonal_coordinates = []

        if self.x_coordinates.index(ship_coordinate[0]) - 1 >= 0:
            diagonal_x_coordinates.append(self.x_coordinates[self.x_coordinates.index(ship_coordinate[0]) - 1])
        try:
            diagonal_x_coordinates.append(self.x_coordinates[self.x_coordinates.index(ship_coordinate[0]) + 1])
        except IndexError:
            pass

        if self.y_coordinates.index(ship_coordinate[1:]) - 1 >= 0:
            diagonal_y_coordinates.append(self.y_coordinates[self.y_coordinates.index(ship_coordinate[1:]) - 1])
        try:
            diagonal_y_coordinates.append(self.y_coordinates[self.y_coordinates.index(ship_coordinate[1:]) + 1])
        except IndexError:
            pass

        for x_coordinate in diagonal_x_coordinates:
            for y_coordinate in diagonal_y_coordinates:
                combined_diagonal_coordinates.append(x_coordinate + y_coordinate)

        return combined_diagonal_coordinates

    def cross_coordinate_getter(self, sender_coordinate=None):
        if sender_coordinate is None:
            sender_coordinate = self.window.ui.centralwidget.sender().objectName()[3:]
        cross_x_coordinates = []
        cross_y_coordinates = []
        combined_cross_coordinates = []

        cross_x_coordinates.append(sender_coordinate[0])

        if self.y_coordinates.index(sender_coordinate[1:]) - 1 >= 0:
            cross_y_coordinates.append(self.y_coordinates[self.y_coordinates.index(sender_coordinate[1:]) - 1])
        try:
            cross_y_coordinates.append(self.y_coordinates[self.y_coordinates.index(sender_coordinate[1:]) + 1])
        except IndexError:
            pass

        for x_coordinate in cross_x_coordinates:
            for y_coordinate in cross_y_coordinates:
                combined_cross_coordinates.append(x_coordinate + y_coordinate)

        cross_x_coordinates = []
        cross_y_coordinates = []

        if self.x_coordinates.index(sender_coordinate[0]) - 1 >= 0:
            cross_x_coordinates.append(self.x_coordinates[self.x_coordinates.index(sender_coordinate[0]) - 1])
        try:
            cross_x_coordinates.append(self.x_coordinates[self.x_coordinates.index(sender_coordinate[0]) + 1])
        except IndexError:
            pass

        cross_y_coordinates.append(sender_coordinate[1:])

        for x_coordinate in cross_x_coordinates:
            for y_coordinate in cross_y_coordinates:
                combined_cross_coordinates.append(x_coordinate + y_coordinate)

        return combined_cross_coordinates

    def ship_size_available(self):
        sender = self.window.ui.ownButtonFrame.sender()
        self.available_sizes = []
        existing_ships_sizes = []

        for item in self.existing_ships_list:
            existing_ships_sizes.append(len(item))
        for size in self.possible_ship_sizes:
            self.available_sizes.append(size)
        for size in existing_ships_sizes:
            try:
                self.available_sizes.remove(size)
            except ValueError:
                continue

        size_to_check = 0

        for coordinate in self.cross_coordinate_getter():
            if self.window.ui.ownButtonFrame.findChild(QtWidgets.QPushButton, "own" + coordinate).property("hasShip"):
                ship_nearby_coordinate = "own" + coordinate
                size_to_check = self.window.ui.ownButtonFrame.findChild(QtWidgets.QPushButton,
                                                                        ship_nearby_coordinate).property("shipSize")
        if len(self.available_sizes) == 1 and size_to_check < self.available_sizes[0]:
            return True
        elif size_to_check + 1 in self.available_sizes:
            return True
        else:
            return False

    def enemy_fleet_generator(self):
        still_available_coordinates = []
        enemy_ship_list_internal = []

        for x_coordinate in self.x_coordinates:
            for y_coordinate in self.y_coordinates:
                still_available_coordinates.append(x_coordinate + y_coordinate)

        for size in self.possible_ship_sizes:

            for ship in enemy_ship_list_internal:
                for coordinate in ship:
                    try:
                        still_available_coordinates.remove(coordinate)
                    except ValueError:
                        continue

                    for diagonal_coordinate in self.diagonal_coordinate_getter(coordinate):
                        try:
                            still_available_coordinates.remove(diagonal_coordinate)
                        except ValueError:
                            continue

                    for cross_coordinate in self.cross_coordinate_getter(ship[0]):
                        try:
                            still_available_coordinates.remove(cross_coordinate)
                        except ValueError:
                            continue
                    for cross_coordinate2 in self.cross_coordinate_getter(ship[-1]):
                        try:
                            still_available_coordinates.remove(cross_coordinate2)
                        except ValueError:
                            continue

            vertical_or_horizontal = random.randint(0, 1)

            new_ship = []

            while len(new_ship) != size:
                new_ship = []
                ship_beginning = random.choice(still_available_coordinates)
                new_ship.append(ship_beginning)

                try:
                    if vertical_or_horizontal == 1:
                        for count, ind in enumerate(range(0, size - 1), 1):
                            if (self.x_coordinates.index(ship_beginning[0]) - count) > 0:
                                new_coord = self.x_coordinates[
                                                self.x_coordinates.index(ship_beginning[0]) - count] + ship_beginning[
                                                                                                       1:]

                                if new_coord in still_available_coordinates:
                                    new_ship.append(new_coord)


                    elif vertical_or_horizontal == 0:
                        for count, ind in enumerate(range(0, size - 1), 1):
                            if (self.y_coordinates.index(ship_beginning[1:]) - count) > 0:
                                new_coord = ship_beginning[0] + self.y_coordinates[
                                    self.y_coordinates.index(ship_beginning[1:]) - count]

                                if new_coord in still_available_coordinates:
                                    new_ship.append(new_coord)
                except IndexError:
                    continue

            new_ship.sort()
            enemy_ship_list_internal.append(new_ship)

        self.enemy_ship_list = enemy_ship_list_internal

        for ship in self.enemy_ship_list:
            for coordinate in ship:
                #self.window.ui.enemyButtonFrame.findChild(QtWidgets.QPushButton, "enemy" + coordinate).setStyleSheet(
                 #   "QPushButton {background-color: red;}")
                self.window.ui.enemyButtonFrame.findChild(QtWidgets.QPushButton, "enemy" + coordinate).setProperty(
                    "hasShip", True)

    def enemy_makes_shot(self):
        hit = False
        sink = False

        if self.last_enemy_shot_hit:
            target = random.choice(self.cross_coordinate_getter(self.last_enemy_shot_coordinate))
        else:
            target = random.choice(self.all_possible_enemy_targets)

        if self.window.ui.ownButtonFrame.findChild(QtWidgets.QPushButton, "own" + target).property("hasShip"):
            self.window.ui.ownButtonFrame.findChild(QtWidgets.QPushButton, "own" + target).setText("x")
            hit = True
            self.last_enemy_shot_hit = True

            for coordinate in self.diagonal_coordinate_getter(target):
                try:
                    self.all_possible_enemy_targets.remove(coordinate)
                except ValueError:
                    continue
        else:
            self.window.ui.ownButtonFrame.findChild(QtWidgets.QPushButton, "own" + target)
            self.window.ui.ownButtonFrame.findChild(QtWidgets.QPushButton, "own" + target).setText("○")
            self.last_enemy_shot_hit = False

        self.last_enemy_shot_coordinate = target

        for ship in self.existing_ships_list:
            if target in ship:
                ship.remove(target)

                if ship == []:
                    self.existing_ships_list.remove(ship)
                    sink = True

        if self.existing_ships_list == []:
            self.game_ends(False)
        self.message_system(target, hit, sink)

    def message_system(self, coordinate, hit=False, sink=False):
        message = "Computer shoots at " + coordinate + "."

        if sink:
            message += " THE SHIP HAS SUNK!"
        elif hit:
            message += " IT'S A HIT!"

        self.window.ui.statusbar.showMessage(message, 15000)

    def game_ends(self, player_wins):
        game_over_message = QMessageBox()
        game_over_message.setWindowTitle("Game Over!")

        if player_wins:
            game_over_message.setText("You won!")
        elif not player_wins:
            game_over_message.setText("You lost!")

        game_over_message.setIcon(QMessageBox.Critical)
        game_over_message.setStandardButtons(QMessageBox.Ok)
        game_over_message.buttonClicked.connect(self.switch_to_lobby)

    def switch_to_lobby(self):
        return


if __name__ == "__main__":
    BattleshipsMain()
