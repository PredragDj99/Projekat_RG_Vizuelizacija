   M S B A M L   `   `   `  ����H  DVizuelizacija, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:VizuelizacijaVizuelizacija  5   	       Vizuelizacija.EditElipsa   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:Vizuelizacija     5   	     � Title$  
EditElipsa��5   	   $	��450��6   $	��450��6)   .��5	      � .+�5
   
   �� $!��Unesite poluprecnik visine:��6   $
��Left=�66   $��	49,40,0,0q�6Q   $	��Top=�6d   �� 5   
   $!��Unesite poluprecnik sirine:��6   $
��Left=�66   $��	53,93,0,0q�6Q   $	��Top=�6d   �� 5   
   $'��!Unesite debljinu konturne linije:��6   $
��Left=�6<   $��
49,146,0,0q�6W   $	��Top=�6k   �� 5   
   $��Izaberite boju elipse: ��6   $
��Left=�62   $��
53,199,0,0q�6M   $	��Top=�6a   �� 5   
   $*��$Izaberite providnost elipse(0-95%): ��6   $
��Left=�6?   $��
53,246,0,0q�6Z   $	��Top=�6n   �� 5   
   $$��Tekst unutar elipse(opciono): ��6   $
��Left=�69   $��
49,305,0,0q�6T   $	��Top=�6h   �� 5   
   $��Boja teksta(opciono): ��6   $
��Left=�61   $��
49,336,0,0q�6L   $	��Top=�6`   �� 5   
   -   5         ��Name$ poluprecnikHeight��5      $
��Left=�6-   $��23��6H   $��
260,40,0,0q�6T    �� TextWrapping$
 Wrap=�6h   $	��Top=�6|   $	��151��6�   �� 5   
   -   5        $ poluprecnikWidth��5      $
��Left=�6,   $��23��6G   $��
260,93,0,0q�6S   $
 Wrap=�6g   $	��Top=�6{   $	��151��6�   �� 5   
   -   5        $ debljinaKonturneLinije��5      $
��Left=�62   $��23��6M   $��260,146,0,0q�6Y   $
 Wrap=�6n   $	��Top=�6�   $	��151��6�   �� 5   
   -   5        $ 
providnost��5      $
��Left=�6&   $��23��6A   $��260,246,0,0q�6M   $
 Wrap=�6b   $	��Top=�6v   $	��151��6�   �� 5   
   -   5        $ tekstUnutarElipse��5      $
��Left=�6-   $��23��6H   $��260,305,0,0q�6T   $
 Wrap=�6i   $	��Top=�6}   $	��151��6�   �� 5   
   -   5         ��Name$ lblHeightGreska��5      $
��Left=�6)   $��
260,63,0,0q�6D   $	��Top=�6X   $��25��6p   $	��151��6|   �� 5   
   -   5        $ lblWidthGreska��5      $
��Left=�6(   $��260,116,0,0q�6C   $	��Top=�6X   $��25��6p   $	��151��6|   �� 5   
   -   5        $ lblKonturnaLinijaGreska��5      $
��Left=�61   $��260,169,0,0q�6L   $	��Top=�6a   $	��151��6y   $��25��6�   �� 5   
   -	   5        $ lblcmbGreska��5      $
��Left=�6&   $��260,221,0,0q�6A   $	��Top=�6V   $	��151��6n   $��25��6z   �� 5   
   -
   5        $ lblProvidnost��5      $
��Left=�6'   $��260,269,0,0q�6B   $	��Top=�6W   $	��151��6o   $��25��6{   �� 5   
   -   5        $��Nacrtaj elipsu��5      $
��Left=�6*   $��175,373,0,0q�6E   $	��Top=�6Z   $��95��6r   $��37��6}   6�   �� 5    
   -   5        $��cmbColor��5       $
��Left=�6P   $��260,199,0,0q�6k   $	��Top=�6�   $	��151��6�   ��5!      �� 5"      .R�5#      �� .+�5$      �� $��92��6$   $��20��6/   ��6;   �� *Name+��6W   �� *Name+5%      5&      5(      �� 5*   
   -   5        $��	cmbColor2��5*      $
��Left=�6Q   $��260,340,0,0q�6l   $	��Top=�6�   $	��151��6�   ��5+      �� 5,      .R�5-      �� .+�5.      �� $��92��6$   $��20��6/   ��6;   �� *Name+��6W   �� *Name+5/      50      52      53      54      