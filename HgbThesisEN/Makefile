file=main
texfile=$(file).tex
pdffile=$(file).pdf

pdf:
	latexmk $(texfile) -pdf -f

open:
	evince $(pdffile) & 

clean: 
	rm *.fls & rm *.aux & rm *.fdb_latexmk & rm *.log
