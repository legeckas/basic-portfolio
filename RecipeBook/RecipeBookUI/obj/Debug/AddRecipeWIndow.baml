   M S B A M L   `   `   `  ����G  CRecipeBookUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:RecipeBookUIRecipeBookUI  5   	   "    RecipeBookUI.AddRecipeWIndow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:RecipeBookUI     5   	     � Title$  Add Recipe - Recipe Book��5   	   $	��650��6*   $	��245��67   .��5	      �  � ColumnDefinitions 5
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
   �� $��Add New Recipe��6   $��1��6*   $��2��6:   $��1��6N   ���	#FF079992$��24(�6r   �� 5%   
   $
��Name��6   $��1��6    $��2��60   $��0,10,5,0q�6=   $��Right=�6O   $��Center=�6k    �� TextAlignment$ Center=�6�   �� 5&   
   -   5         ��Name$ addRecipeNameTextBox��5&      $��2��60   $��2��6@   $	��100��6M   $��0,10,0,0q�6\   �� 5(   
   $��Prep time (mins)��6   $��1��6,   $��3��6<   $��0,10,5,0q�6I   $��Right=�6[   $��Center=�6w   $ Center=�6�   �� 5)   
   -   5        $ addRecipePrepTimeTextBox��5)      $��2��64   $��3��6D   $	��100��6Q   $��0,10,0,0q�6`   �� 5+   
   $��Ingredients��6   $��1��6'   $��2��67   $��4��6K   $��0,10,0,0q�6X   �� 5,   
   -   5         ��Name$" addRecipeAddIngredientButton��5,      $	��Add��67   $��1��6E   $��5��6U   $��0,5,2,0q�6b   $��20��6s   $	��Top=�6   6�   �� 5-   
   -   5        $% addRecipeRemoveIngredientButton��5-      $��Remove��6:   $��2��6K   $��5��6[   $��2,5,0,0q�6h   6y   �� 5.   
   $��1��6   $��2��6'   $��6��6;   $��0,10,0,0q�6H   .��5/      �� -   5         ��Name$&  addRecipeAddedIngredientsListbox��5/      $	��100��6@    �� SelectedValue 6P   �� *Name+50      �� 52   
   $��Preparation instructions��6   $��1��64   $��2��6D   $��7��6X   $��0,10,0,0q�6e   �� 53   
   $��1��6   $��2��6'   $��8��6;   $��0,5,0,0q�6H   .��54      �� -   5        $" addRecipeInstructionsTextBox��54      $	��200��6<    �� TextWrapping$
 Wrap=�6L   	 �� AcceptsReturn	 . 6`   55      �� 57   
   -   5        $ addRecipeAddImageButton��57      $��Add Image...��62   $��1��6J   $��2��6Z   $��9��6n   $��0,10,0,0q�6{   6�   �� 58   
   -   5        
 ��Name$$
 addRecipeAddImagePathTextBlock��58      $��1��6<   $��2��6L   $��10��6`   $��0,10,0,0q�6n   $
��Wrap=�6�   $��Stretch=�6�   �� 5:   
   -	   5        $! addRecipeAddNewRecipeButton��5:      $��Add New Recipe��66   $��1��6O   $��2��6_   $��11��6s   $��0,15,0,0q�6�   6�   5;      5<      