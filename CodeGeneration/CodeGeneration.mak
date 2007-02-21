CodeGenerator = ..\CodeGenerator\bin\debug\CodeGenerator.exe
template = ..\CodeGenerator\template.txt
outdir = ..\IETest

default: rebuild

build: $(outdir)\TableCollection.cs 
build: $(outdir)\TableSectionCollection.cs 
build: $(outdir)\TableRowCollection.cs 
build: $(outdir)\TableCellCollection.cs 
build: $(outdir)\SpanCollection.cs 
build: $(outdir)\DivCollection.cs 
build: $(outdir)\FormCollection.cs 
build: $(outdir)\InputCollection.cs 
build: $(outdir)\InputButtonCollection.cs 
build: $(outdir)\ImgCollection.cs 
build: $(outdir)\AnchorCollection.cs
build: $(outdir)\SelectCollection.cs
build: $(outdir)\OptionCollection.cs

$(outdir)\TableCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) Table HTMLTable $(outdir)

$(outdir)\TableSectionCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) TableSection HTMLTableSection $(outdir)

$(outdir)\TableRowCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) TableRow HTMLTableRow $(outdir)

$(outdir)\TableCellCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) TableCell HTMLTableCell $(outdir)

$(outdir)\SpanCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) Span HTMLSpanElement $(outdir)

$(outdir)\DivCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) Div HTMLDivElement $(outdir)

$(outdir)\FormCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) Form HTMLFormElement $(outdir)

$(outdir)\InputCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) Input HTMLInputElement $(outdir)

$(outdir)\InputButtonCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) InputButton HTMLInputButtonElement $(outdir)

$(outdir)\ImgCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) Img HTMLImg $(outdir)

$(outdir)\AnchorCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) Anchor HTMLAnchorElement $(outdir)

$(outdir)\SelectCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) Select HTMLSelectElement $(outdir)
	
$(outdir)\OptionCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) Option HTMLOptionElement $(outdir)

$(outdir)\OptionCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) Option HTMLOptionElement $(outdir)

clean:

rebuild: clean build
	

