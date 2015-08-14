# Written by Marco Comini

PDFLATEX = pdflatex --interaction=nonstopmode
MAKEINDEX = makeindex

.PRECIOUS: %.pdf %.bbl

generatedPDF = $(patsubst %.dtx,%.pdf,$(wildcard *.dtx)) $(patsubst %.x,%.hs,$(wildcard *.x))

.PHONY : all
all: $(generatedPDF)

.PHONY : clean
clean:
	-rm -fv $(generatedPDF) *.{sty,cls,sdf,aux,log,ilg,toc,out,idx,ind}


%.pdf: %.dtx %.ins
	$(PDFLATEX) $*.ins
	$(PDFLATEX) $*.dtx >/dev/null
	$(MAKEINDEX) -s $*.ist $* >/dev/null
	$(PDFLATEX) $*.dtx >/dev/null
	$(PDFLATEX) $*.dtx >/dev/null
	-rm -f $*.{aux,log,ilg,toc,out,idx,ind}
