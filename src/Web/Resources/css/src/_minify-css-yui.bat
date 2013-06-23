#ECHO OFF
CLS

java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar -v --charset utf-8 -o admin.form.min.css     admin.form.css
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar -v --charset utf-8 -o admin.layout.min.css     admin.layout.css
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar -v --charset utf-8 -o admin.layout2col-alt.min.css     admin.layout2col-alt.css
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar -v --charset utf-8 -o admin.layout2col.min.css     admin.layout2col.css
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar -v --charset utf-8 -o admin.reset.min.css      admin.reset.css 
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar -v --charset utf-8 -o admin.style.min.css     admin.style.css
java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar -v --charset utf-8 -o admin.ui.common.min.css     admin.ui.common.css

#java -jar ..\..\..\..\..\Tools\YUICompressor\build\yuicompressor.jar -v --charset utf-8 -o ui.daterangepicker.min.css     ui.daterangepicker.css


ECHO Minifization done (YUICompressor Version)!
ECHO Premi un tasto per spostare i file min nella cartella js...
PAUSE 
MOVE /Y *.min.css ..

ECHO DONE!!!

PAUSE
