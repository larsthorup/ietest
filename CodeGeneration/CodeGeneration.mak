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
	$(CodeGenerator) Table HTMLTable

$(outdir)\TableSectionCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) TableSection HTMLTableSection

$(outdir)\TableRowCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) TableRow HTMLTableRow

$(outdir)\TableCellCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) TableCell HTMLTableCell

$(outdir)\SpanCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) Span HTMLSpanElement

$(outdir)\DivCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) Div HTMLDivElement

$(outdir)\FormCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) Form HTMLFormElement

$(outdir)\InputCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) Input HTMLInputElement

$(outdir)\InputButtonCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) InputButton HTMLInputButtonElement

$(outdir)\ImgCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) Img HTMLImg

$(outdir)\AnchorCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) Anchor HTMLAnchorElement

$(outdir)\SelectCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) Select HTMLSelectElement

$(outdir)\OptionCollection.cs: $(template) $(CodeGenerator)
	$(CodeGenerator) Option HTMLOptionElement

clean:

rebuild: clean build
	

