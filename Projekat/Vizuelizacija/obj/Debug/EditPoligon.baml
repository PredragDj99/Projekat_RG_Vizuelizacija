   M S B A M L   `   `   `  ����H  DVizuelizacija, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:VizuelizacijaVizuelizacija  5   	       Vizuelizacija.EditPoligon   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:Vizuelizacija     5   	     � Title$  EditPolygon��5   	   $	��450��6   $	��450��6*   .��5	      � .+�5
   
   �� $'��!Unesite debljinu konturne linije:��6   $
��Left=�6<   $��	51,51,0,0q�6W   $	��Top=�6j   �� 5   
   $��Izaberite boju poligona: ��6   $
��Left=�64   $��
53,112,0,0q�6O   $	��Top=�6c    J� RenderTransformOrigin$ 0.519,-0.281&�6{   �� 5   
   $&�� Tekst unutar poligona(opciono): ��6   $
��Left=�6;   $��
51,236,0,0q�6V   $	��Top=�6j   �� 5   
   $��Boja teksta(opciono): ��6   $
��Left=�61   $��
53,281,0,0q�6L   $	��Top=�6`   �� 5   
   $,��&Izaberite providnost poligona(0-95%): ��6   $
��Left=�6A   $��
53,172,0,0q�6\   $	��Top=�6p   $ 0.519,-0.281&�6�   �� 5   
   -   5         ��Name$ debljinaKonturneLinije��5      $
��Left=�62   $��23��6M   $��
260,54,0,0q�6Y    �� TextWrapping$
 Wrap=�6m   $	��Top=�6�   $	��151��6�   �� 5   
   -   5        $ tekstUnutarPoligona��5      $
��Left=�6/   $��23��6J   $��260,239,0,0q�6V   $
 Wrap=�6k   $	��Top=�6   $	��151��6�   �� 5   
   -   5        $ 
providnost��5      $
��Left=�6&   $��23��6A   $��260,172,0,0q�6M   $
 Wrap=�6b   $	��Top=�6v   $	��151��6�   �� 5   
   -   5         ��Name$ lblKonturnaLinijaGreska��5      $
��Left=�61   $��
260,82,0,0q�6L   $	��Top=�6`   $	��151��6x   $��25��6�   �� 5   
   -   5        $ lblcmdGreska��5      $
��Left=�6&   $��260,139,0,0q�6A   $	��Top=�6V   $	��151��6n   $��25��6z   �� 5   
   -   5        $ lblProvidnost��5      $
��Left=�6'   $��260,195,0,0q�6B   $	��Top=�6W   $	��151��6o   $��25��6{   �� 5   
   -   5        $��Nacrtaj poligon��5      $
��Left=�6+   $��175,373,0,0q�6F   $	��Top=�6[   $��95��6s   $��37��6~   6�   �� 5   
   -   5         ��Name$ cmbColor��5      $
��Left=�6R   $��260,112,0,0q�6m   $	��Top=�6�   $	��151��6�   ��5      �� 5      .R�5      �� .+�5      �� $��92��6$   $��20��6/   ��6;   �� *Name+��6W   �� *Name+5      5       5"      �� 5$   
   -	   5        $ 	cmbColor2��5$      $
��Left=�6S   $��260,285,0,0q�6n   $	��Top=�6�   $	��151��6�   ��5%      �� 5&      .R�5'      �� .+�5(      �� $��92��6$   $��20��6/   ��6;   �� *Name+��6W   �� *Name+5)      5*      5,      5.      5/      