   M S B A M L   `   `   `  ����G  CRecipeBookUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:RecipeBookUIRecipeBookUI  5   	   &     RecipeBookUI.AddIngredientWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:RecipeBookUI     5   	     � Title$#  Add Ingredients - Recipe Book��5   	   $	��285��6/   $	��215��6<   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��20 �6   �� 5      $
��auto �6   �� 5      $��* �6   �� 5      $��20 �6    � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��* �6   �� 5      $��20 �6   .+�5   
   �� $��Add Ingredient��6   $��1��6*   $��2��6:   $��1��6N   ���	#FF079992$��24(�6r   �� 5    
   $��
Ingredient��6   $��1��6&   $��2��66   $��5,10,0,0q�6C   �� 5!   
   -   5         ��Name$% addIngredientsAvailableComboBox��5!      $��1��6<   $��3��6L   $��5,5,0,0q�6Y   $	��150��6j   �� 5#   
   -   5         ��Name$ addIngredientsNewCheckBox��5#      $��
Add new...��66   $��1��6K   $��4��6[   $��5,10,0,0q�6h   6�   �� 5$   
   -   5         ��Name$" addIngredientsNewNameTextBox��5$      $��1��68   $��5��6H   $��5,5,0,0q�6U   �� 5&   
   $��Amount��6   $��1��6"   $��6��62   $��5,10,0,0q�6?   �� 5'   
   -   5        $! addIngredientsAmountTextBox��5'      $��1��67   $��7��6G   $��5,5,0,0q�6T   �� 5)   
   -   5         ��Name$ addIngredientsAddButton��5)      $��Add Ingredient��62   $��1��6K   $��8��6[   $��5,15,0,0q�6h   6z   5+      5,      