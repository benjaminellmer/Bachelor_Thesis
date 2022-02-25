file=main
texfile=$(file).tex
pdffile=$(file).pdf

pdf:
	latexmk $(texfile) -pdf

open:
	evince $(pdffile) & 

clean: 
	rm *.fls & rm *.aux & rm *.fdb_latexmk & rm *.log
