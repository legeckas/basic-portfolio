   M S B A M L   `   `   `  ����G  CRecipeBookUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:RecipeBookUIRecipeBookUI  5   	   #    RecipeBookUI.EditRecipeWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:RecipeBookUI     5   	     � Title$  Edit Recipe - Recipe Book��5   	   $	��650��6+   $	��245��68   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��20 �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��* �6   �� 5      $��20 �6    � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��* �6   �� 5       $��20 �6   .+�5#   
   �� $��Edit Recipe��6   $��1��6'   $��2��67   $��1��6K   ���	#FF079992$��24(�6o   �� 5%   
   $
��Name��6   $��1��6    $��2��60   $��0,10,5,0q�6=   $��Right=�6O   $��Center=�6k    �� TextAlignment$ Center=�6�   �� 5&   
   -   5         ��Name$ editRecipeNameTextBox��5&      $��2��61   $��2��6A   $	��100��6N   $��0,10,0,0q�6]   �� 5(   
   $��Prep time (mins)��6   $��1��6,   $��3��6<   $��0,10,5,0q�6I   $��Right=�6[   $��Center=�6w   $ Center=�6�   �� 5)   
   -   5        $ editRecipePrepTimeTextBox��5)      $��2��65   $��3��6E   $	��100��6R   $��0,10,0,0q�6a   �� 5+   
   $��Ingredients��6   $��1��6'   $��2��67   $��4��6K   $��0,10,0,0q�6X   �� 5,   
   -   5         ��Name$# editRecipeAddIngredientButton��5,      $	��Add��68   $��1��6F   $��5��6V   $��0,5,2,0q�6c   $��20��6t   $	��Top=�6�   6�   �� 5-   
   -   5        $&  editRecipeRemoveIngredientButton��5-      $��Remove��6;   $��2��6L   $��5��6\   $��2,5,0,0q�6i   6z   �� 5.   
   $��1��6   $��2��6'   $��6��6;   $��0,10,0,0q�6H   .��5/      �� -   5         ��Name$' !editRecipeAddedIngredientsListbox��5/      $	��100��6A    �� SelectedValue 6Q   �� *Name+50      �� 52   
   $��Preparation instructions��6   $��1��64   $��2��6D   $��7��6X   $��0,10,0,0q�6e   �� 53   
   $��1��6   $��2��6'   $��8��6;   $��0,5,0,0q�6H   .��54      �� -   5        $# editRecipeInstructionsTextBox��54      $	��200��6=    �� TextWrapping$
 Wrap=�6M   	 �� AcceptsReturn	 . 6a   55      �� 57   
   -   5        $ editRecipeAddImageButton��57      $��Add Image...��63   $��1��6K   $��2��6[   $��9��6o   $��0,10,0,0q�6|   6�   �� 58   
   -   5        
 ��Name$%
 editRecipeAddImagePathTextBlock��58      $��1��6=   $��2��6M   $��10��6a   $��0,10,0,0q�6o   $
��Wrap=�6�   $��Stretch=�6�   �� 5:   
   -	   5        $" editRecipeUpdateRecipeButton��5:      $��Update Recipe��67   $��1��6O   $��2��6_   $��11��6s   $��0,15,0,0q�6�   6�   5;      5<      