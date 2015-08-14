%%
%% This is file `PhdThesis.cls',
%% generated with the docstrip utility.
%%
%% The original source files were:
%%
%% PhdThesis.dtx  (with options: `class')
%% 
%% Copyright (C) 1995-2004 Marco Comini <comini@dimi.uniud.it>.
%% 
%% This class is the Phd Thesis format of the Computer Science Deartment of
%% the Pisa and Udine Universities -- Italy.
%% 
%% This class is distributed in the hope that it will be useful, but WITHOUT
%% ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
%% FITNESS FOR A PARTICULAR PURPOSE.
%% 
%% Direct use of code from this class in another class which is either sold
%% for profit, or not made freely available, is explicitly forbidden by the
%% author.
%% 
\NeedsTeXFormat{LaTeX2e}[1995/12/01]
\ProvidesClass{PhdThesis}[2004/02/17 2.0h PhdThesis document class]
%%%%%
%%%%% Options
%%%%%
\DeclareOption{a5paper}
{\ClassWarningNoLine{PhdThesis}{Option `a5paper' not supported}}
\DeclareOption{b5paper}
{\ClassWarningNoLine{PhdThesis}{Option `b5paper' not supported}}
\DeclareOption{letterpaper}
{\ClassWarningNoLine{PhdThesis}{Option `letterpaper' not supported}}
\DeclareOption{legalpaper}
{\ClassWarningNoLine{PhdThesis}{Option `legalpaper' not supported}}
\DeclareOption{executivepaper}
{\ClassWarningNoLine{PhdThesis}{Option `executivepaper' not supported}}
\DeclareOption{landscape}
{\ClassWarningNoLine{PhdThesis}{Option `landscape' not supported}}
\DeclareOption{titlepage}
{\ClassWarningNoLine{PhdThesis}{Option `titlepage' not supported}}
\DeclareOption{twocolumn}
{\ClassWarningNoLine{PhdThesis}{Option `twocolumn' not supported}}
\DeclareOption{final}{}
\ExecuteOptions{final}
\DeclareOption{draft}{\PassOptionsToClass{draft}{book}}
\DeclareOption{a4paper}{\PassOptionsToClass{a4paper}{book}}
\ExecuteOptions{a4paper}
\DeclareOption{onecolumn}{\PassOptionsToClass{onecolumn}{book}}
\ExecuteOptions{onecolumn}
%%% Flag phd@langdef
\newif\if@phd@langdef\@phd@langdeffalse
\DeclareOption{english}{\@phd@langdeftrue\AtBeginDocument{
\phd@setcaptions{english}{\phd@captions@english}
\typeout{Class PhdThesis: Adding British English Captions.}
}}
\DeclareOption{american}{\@phd@langdeftrue\AtBeginDocument{
\phd@setcaptions{american}{\phd@captions@american}
\typeout{Class PhdThesis: Adding American English Captions.}
}}
\DeclareOption{italian}{\@phd@langdeftrue\AtBeginDocument{
\phd@setcaptions{italian}{\phd@captions@italian}
\typeout{Class PhdThesis: Adding Italian Captions.}
}}
\DeclareOption{spanish}{\@phd@langdeftrue\AtBeginDocument{
\phd@setcaptions{spanish}{\phd@captions@spanish}
\typeout{Class PhdThesis: Adding Spanish Captions.}
}}
\DeclareOption{french}{\@phd@langdeftrue\AtBeginDocument{
\phd@setcaptions{french}{\phd@captions@french}
\typeout{Class PhdThesis: Adding French Captions.}
}}
%%% Flag phd@chpempty
\newif\if@phd@chpempty
\DeclareOption{chapterempty}{\@phd@chpemptytrue}
\ExecuteOptions{chapterempty}
\DeclareOption{chapterplain}{\@phd@chpemptyfalse}
%%% Global phd@shapefile
\let\@phd@shapefile\@empty
\DeclareOption{UdinePhdThesis}{\def\@phd@shapefile{UdinePhdThesis}
\PassOptionsToClass{twoside}{book}}
\ExecuteOptions{UdinePhdThesis}
\DeclareOption{UdinePhdThesisCR}{\def\@phd@shapefile{UdinePhdThesisCR}
\PassOptionsToClass{twoside}{book}}
\DeclareOption{UdineMstThesis}{\def\@phd@shapefile{UdineMstThesis}
\PassOptionsToClass{twoside}{book}}
\DeclareOption{PisaPhdThesis}{\def\@phd@shapefile{PisaPhdThesis}
\PassOptionsToClass{twoside}{book}}
\DeclareOption{PisaMasterThesis}{\def\@phd@shapefile{PisaMstThesis}
\PassOptionsToClass{oneside}{book}}
\DeclareOption{ValenciaPhdThesis}{\def\@phd@shapefile{ValenciaPhdThesis}
\PassOptionsToClass{twoside}{book}}
\DeclareOption{VeneziaPhdThesis}{\def\@phd@shapefile{VeneziaPhdThesis}
\PassOptionsToClass{twoside}{book}}
\DeclareOption*{\PassOptionsToClass{\CurrentOption}{book}}
\InputIfFileExists{PhdThesis.cfg}{}{\ClassWarningNoLine{PdhThesis}%
{No preferences file}}
\ProcessOptions\relax
\if@phd@langdef\else
\ClassWarningNoLine{PhdThesis}{no language has been specified,
English will be used}
\AtBeginDocument{
\phd@setcaptions{english}{\phd@captions@english}
\typeout{Class PhdThesis: Adding British English Captions.}
}
\fi
\LoadClass{book}
%%%%%
%%%%% New Headings & Chapter styles
%%%%%
\newcommand{\@phd@newpagestyle}[2]{%
\@namedef{ps@#1}{\let\@oddfoot\@empty\let\@evenfoot\@empty
\let\@mkboth\@phd@mkboth#2}}
\if@phd@chpempty
\let\ps@latexplain\ps@plain
\let\ps@plain\ps@empty
\fi
\if@twoside
\newcommand*{\phd@chaptermark}[1]{\markboth{#1}{}}
\newcommand*{\phd@sectionmark}[1]{\markright{#1}}
\let\@phd@mkboth\markboth
\else
\newcommand*{\phd@chaptermark}[1]{\markright{#1}}
\newcommand*{\phd@sectionmark}[1]{}
\let\@phd@mkboth\@gobbletwo
\fi
\AtBeginDocument{\pagestyle{small}}
\@phd@newpagestyle{small}{%
\def\@evenhead{\thepage\hfil\footnotesize\leftmark}
\def\@oddhead{{\footnotesize\rightmark}\hfil\thepage}
\def\chaptermark##1{%
\phd@chaptermark{\MakeUppercase{%
\ifnum \c@secnumdepth >\m@ne
\if@mainmatter
\@chapapp\ \thechapter. \ %
\fi
\fi
##1}}}%
\def\sectionmark##1{%
\phd@sectionmark{\MakeUppercase{%
\ifnum \c@secnumdepth >\z@
\if@mainmatter
\thesection. \ %
\fi
\fi
##1}}}}
\@phd@newpagestyle{underlined}{%
\def\@evenhead{\underline{\makebox[\textwidth]
{{\small\bfseries\thepage\hfill\leftmark}
\rule[-0.5ex]{0pt}{1ex}}}}%
\def\@oddhead{\underline{\makebox[\textwidth]
{{\small\bfseries\rightmark\hfill\thepage}
\rule[-0.5ex]{0pt}{1ex}}}}%
\def\chaptermark##1{%
\phd@chaptermark{\MakeUppercase{%
\ifnum \c@secnumdepth >\m@ne
\if@mainmatter
\@chapapp\ \thechapter. \ %
\fi
\fi
##1}}}%
\def\sectionmark##1{%
\phd@sectionmark{\MakeUppercase{%
\ifnum \c@secnumdepth >\z@
\if@mainmatter
\thesection. \ %
\fi
\fi
##1}}}}
\@phd@newpagestyle{mini}{%
\def\@evenhead{\underline{\makebox[\textwidth]
{{\small\bfseries\thepage\hfill\leftmark}
\rule[-0.5ex]{0pt}{1ex}}}}%
\def\@oddhead{\underline{\makebox[\textwidth]
{{\small\bfseries\rightmark\hfill\thepage}
\rule[-0.5ex]{0pt}{1ex}}}}%
\def\chaptermark##1{%
\phd@chaptermark{\MakeUppercase{%
\ifnum \c@secnumdepth >\m@ne
\if@mainmatter
\thechapter. \ %
\fi
\fi
##1}}}%
\def\sectionmark##1{%
\phd@sectionmark{\MakeUppercase{%
\ifnum \c@secnumdepth >\z@
\if@mainmatter
\thesection. \ %
\fi
\fi
##1}}}}
\@phd@newpagestyle{center}{%
\def\@evenhead{\thepage\hfil{\footnotesize\itshape\leftmark}\hfil}
\def\@oddhead{\hfil{\footnotesize\itshape\rightmark}\hfil\thepage}
\def\chaptermark##1{\phd@chaptermark{\MakeUppercase{##1}}}%
\def\sectionmark##1{\phd@sectionmark{\MakeUppercase{##1}}}}
\@phd@newpagestyle{centersmallcaps}{%
\def\@evenhead{\thepage\hfil{\footnotesize\scshape\leftmark}\hfil}
\def\@oddhead{\hfil{\footnotesize\scshape\rightmark}\hfil\thepage}
\def\chaptermark##1{\phd@chaptermark{##1}}%
\def\sectionmark##1{\phd@sectionmark{##1}}}
\@phd@newpagestyle{serif}{%
\def\@evenhead{\underline{\makebox[\textwidth]
{{\small\sffamily\bfseries\thepage\hfill\leftmark}
\rule[-0.5ex]{0pt}{1ex}}}}%
\def\@oddhead{\underline{\makebox[\textwidth]
{{\small\sffamily\bfseries\rightmark\hfill\thepage}
\rule[-0.5ex]{0pt}{1ex}}}}%
\def\chaptermark##1{%
\phd@chaptermark{%\MakeUppercase{
\ifnum \c@secnumdepth >\m@ne
\if@mainmatter
\thechapter. \ %
\fi
\fi
##1}}%}
\def\sectionmark##1{%
\phd@sectionmark{%\MakeUppercase{
\ifnum \c@secnumdepth >\z@
\if@mainmatter
\thesection. \ %
\fi
\fi
##1}}}%}
\@phd@newpagestyle{smallcaps}{%
\def\@evenhead{\makebox[\textwidth]
{{\small\scshape\thepage\hfill\leftmark}
\rule[-0.5ex]{0pt}{1ex}}}%
\def\@oddhead{\makebox[\textwidth]
{{\small\scshape\rightmark\hfill\thepage}
\rule[-0.5ex]{0pt}{1ex}}}%
\def\chaptermark##1{%
\phd@chaptermark{%\MakeUppercase{
\ifnum \c@secnumdepth >\m@ne
\if@mainmatter
\thechapter. \ %
\fi
\fi
##1}}%}
\def\sectionmark##1{%
\phd@sectionmark{%\MakeUppercase{
\ifnum \c@secnumdepth >\z@
\if@mainmatter
\thesection. \ %
\fi
\fi
##1}}}%}
\@phd@newpagestyle{lowercase}{%
\def\@evenhead{\thepage\hfil\normalsize\leftmark}
\def\@oddhead{{\normalsize\rightmark}\hfil\thepage}
\def\chaptermark##1{%
\phd@chaptermark{
\ifnum \c@secnumdepth >\m@ne
\if@mainmatter
\@chapapp\ \thechapter. \ %
\fi
\fi
##1}}%
\def\sectionmark##1{%
\phd@sectionmark{
\ifnum \c@secnumdepth >\z@
\if@mainmatter
\thesection. \ %
\fi
##1
\fi}}}
\@phd@newpagestyle{lowercaseunderlined}{%
\def\@evenhead{\underline{\makebox[\textwidth]
{{\small\bfseries\thepage\hfill\leftmark}
\rule[-0.5ex]{0pt}{1ex}}}}%
\def\@oddhead{\underline{\makebox[\textwidth]
{{\small\bfseries\rightmark\hfill\thepage}
\rule[-0.5ex]{0pt}{1ex}}}}%
\def\chaptermark##1{%
\phd@chaptermark{%
\ifnum \c@secnumdepth >\m@ne
\if@mainmatter
\@chapapp\ \thechapter. \ %
\fi
\fi
##1}}%
\def\sectionmark##1{%
\phd@sectionmark{%
\ifnum \c@secnumdepth >\z@
\if@mainmatter
\thesection. \ %
\fi
##1
\fi}}}
%%% New Styles
\newcommand{\@phd@newstyle}[3]{%
\@namedef{style@#1@#2}{#3}}
%%% New Chapter Titles Styles
\newcommand{\chaptertitlestyle}[1]{%
\@ifundefined{style@chap@#1}%
\undefinedchaptertitlestyle
{\@nameuse{style@chap@#1}%
\typeout{Class PhdThesis: using `#1' chapter-title style.}}}
\let\@phd@plainmakechapterhead\@makechapterhead
\let\@phd@plainmakeschapterhead\@makeschapterhead
\@phd@newstyle{chap}{plain}{%
\let\@makechapterhead\@phd@plainmakechapterhead
\let\@makeschapterhead\@phd@plainmakeschapterhead
}
\@phd@newstyle{chap}{serif}{%
\def\@makechapterhead##1{%
\vspace*{-75\p@}\noindent\rule{\textwidth}{4pt}\par\noindent%
{\parindent \z@ \raggedright \normalfont
\ifnum \c@secnumdepth >\m@ne
\if@mainmatter
\small\sffamily\@chapapp\space\Huge\thechapter
\space\space
\fi
\fi
\interlinepenalty\@M
\Huge \bfseries ##1\par\nobreak
\vskip 80\p@
}}
\def\@makeschapterhead##1{%
\vspace*{-75\p@}\noindent\rule{\textwidth}{4pt}\par\noindent%
{\parindent \z@ \raggedright \normalfont
\interlinepenalty\@M
\Huge \bfseries ##1\par\nobreak
\vskip 80\p@
}}%
}
\@phd@newstyle{chap}{serifbig}{%
\DeclareFontShape{OT1}{cmr}{b}{n}{<-> cmr17}{}
\def\@makechapterhead##1{%
\vspace*{-10ex}%
\noindent\rule{\textwidth}{4pt}\par\nobreak\noindent%
\vskip 5ex%
{\parindent \z@ \raggedleft \normalfont
\ifnum \c@secnumdepth >\m@ne
\if@mainmatter
\fontsize{99.45}{100pt}\fontseries{b}\selectfont\thechapter%
\Huge\par\nobreak%
\vskip 1ex
\fi
\fi
\interlinepenalty\@M \Huge \bfseries\sffamily
##1\par\nobreak \vskip 40\p@ }}
\def\@makeschapterhead##1{%
\vspace*{-10ex}%
\noindent\rule{\textwidth}{4pt}\par\nobreak\noindent%
\vskip 5ex%
  {\parindent \z@ \raggedleft \normalfont
  \interlinepenalty\@M \Huge \bfseries\sffamily
##1\par\nobreak \vskip 40\p@
}}%
}
\@phd@newstyle{chap}{big}{%
\DeclareFontShape{OT1}{cmr}{b}{n}{<-> cmr17}{}
\def\@makechapterhead##1{%
\vspace*{50\p@}%
{\parindent \z@ \raggedleft \normalfont
\ifnum \c@secnumdepth >\m@ne
\if@mainmatter
\fontsize{99.45}{100pt}\fontseries{b}\selectfont\thechapter%
\Huge\par\nobreak\vskip 20\p@
\fi
\fi
\interlinepenalty\@M \Huge \bfseries
##1\par\nobreak \vskip 40\p@ }}
\def\@makeschapterhead##1{%
\vspace*{50\p@}%
{\parindent \z@ \raggedleft \normalfont
\interlinepenalty\@M \Huge \bfseries
##1\par\nobreak \vskip 40\p@
}}%
}
%%% New Part Styles
\newcommand{\partstyle}[1]{%
\@ifundefined{style@part@#1}%
\undefinedpartstyle
{\@nameuse{style@part@#1}%
\typeout{Class PhdThesis: using `#1' part style.}}}
\let\@phd@plainpart\@part
\let\@phd@plainspart\@spart
\@phd@newstyle{part}{plain}{%
\let\@part\@phd@plainpart
\let\@spart\@phd@plainspart
}
\@phd@newstyle{part}{big}{%
\DeclareFontShape{OT1}{cmr}{b}{n}{<-> cmr17}{}
\def\@part[##1]##2{%
\ifnum \c@secnumdepth >-2\relax
\refstepcounter{part}%
\addcontentsline{toc}{part}{\thepart\hspace{1em}##1}%
\else
\addcontentsline{toc}{part}{##1}%
\fi
\markboth{}{}%
{\raggedleft
\interlinepenalty \@M
\normalfont
\ifnum \c@secnumdepth >-2\relax
\if@mainmatter
\fontsize{99.45}{100pt}\fontseries{b}\selectfont%
\thepart
\par
\vskip 20\p@
\hrule
\vskip 20\p@
\fi
\fi
\Huge \bfseries ##2\par}%
\@endpart}
\def\@spart##1{%
{\raggedleft
\interlinepenalty \@M
\normalfont
\hrule
\vskip 20\p@
\Huge \bfseries ##1\par}%
\@endpart}
}
\@phd@newstyle{part}{serifbig}{%
\DeclareFontShape{OT1}{cmr}{b}{n}{<-> cmr17}{}
\def\@part[##1]##2{%
\ifnum \c@secnumdepth >-2\relax
\refstepcounter{part}%
\addcontentsline{toc}{part}{\thepart\hspace{1em}##1}%
\else
\addcontentsline{toc}{part}{##1}%
\fi
\markboth{}{}%
{\raggedleft
\interlinepenalty \@M
\normalfont
\ifnum \c@secnumdepth >-2\relax
\if@mainmatter
\fontsize{99.45}{100pt}\fontseries{b}\selectfont%
\thepart
\par
\vskip 20\p@
\hrule
\vskip 20\p@
\fi
\fi
\Huge\bfseries\sffamily ##2\par}%
\@endpart}
\def\@spart##1{%
{\raggedleft
\interlinepenalty \@M
\normalfont
\hrule
\vskip 20\p@
\Huge \bfseries ##1\par}%
\@endpart}
}
%%%%%
%%%%% Abstract, Preface, Introduction and Acknowledgments pages
%%%%%
%%% Flag phd@afterintro
\newif\if@phd@afterintro\@phd@afterintrofalse
\newcommand*{\preface}{%
\if@twocolumn
\@restonecoltrue\onecolumn
\else
\@restonecolfalse
\fi
\chapter*{\prefacename
\chaptermark{\prefacename}\sectionmark{\prefacename}}%
\ifnum \c@secnumdepth >\m@ne
\if@mainmatter
\ClassWarningNoLine{PhdThesis}{preface environment %
should NOT be used in mainmatter mode}
\fi
\typeout{\prefacename.}%
\addcontentsline{toc}{chapter}
{\protect\numberline{}\prefacename}%
\if@phd@afterintro\else
\global\@phd@afterintrotrue
\global\let\@phd@thechapter\thechapter
\fi
\gdef\thechapter{P}
\setcounter{section}{0}%
\setcounter{subsection}{0}%
\else
\addcontentsline{toc}{chapter}{\prefacename}%
\fi
\if@restonecol\twocolumn\fi
\thispagestyle{empty}
}
\newcommand*{\introduction}{%
\setcounter{section}{0}%
\setcounter{subsection}{0}%
\if@twocolumn
\@restonecoltrue\onecolumn
\else
\@restonecolfalse
\fi
\chapter*{\introductionname
\chaptermark{\introductionname}\sectionmark{\introductionname}}%
\ifnum \c@secnumdepth >\m@ne
\if@mainmatter
\ClassWarningNoLine{PhdThesis}{introduction environment %
should NOT be used in mainmatter mode}
\fi
\typeout{\introductionname.}%
\addcontentsline{toc}{chapter}
{\protect\numberline{}\introductionname}%
\if@phd@afterintro\else
\global\@phd@afterintrotrue
\global\let\@phd@thechapter\thechapter
\fi
\gdef\thechapter{I}
\setcounter{section}{0}%
\setcounter{subsection}{0}%
\else
\addcontentsline{toc}{chapter}{\introductionname}%
\fi
\if@restonecol\twocolumn\fi
\thispagestyle{empty}
}
%%% Flag phd@afterapp
\newif\if@phd@afterapp\@phd@afterappfalse
\newcommand*{\conclusions}{%
\if@twocolumn
\@restonecoltrue\onecolumn
\else
\@restonecolfalse
\fi
\chapter*{\conclusionsname
\chaptermark{\conclusionsname}\sectionmark{\conclusionsname}}%
\ifnum \c@secnumdepth >\m@ne
\if@mainmatter
\ClassWarningNoLine{PhdThesis}{conclusions environment %
should NOT be used in mainmatter mode}
\else\fi
\typeout{\conclusionsname.}%
\addcontentsline{toc}{chapter}
{\protect\numberline{}\conclusionsname}%
\if@phd@afterintro\else
\global\@phd@afterintrotrue
\global\let\@phd@thechapter\thechapter
\fi
\gdef\thechapter{C}
\setcounter{section}{0}%
\setcounter{subsection}{0}%
\if@phd@afterapp
\global\let\thesection\@phd@thesection
\global\@phd@afterappfalse
\fi
\else
\addcontentsline{toc}{chapter}{\conclusionsname}%
\fi
\if@restonecol\twocolumn\fi
\thispagestyle{empty}
}
%%%%%
%%%%% MakeTitle Strings Definition Commands
%%%%%
%%% Init strings
\let\@phd@chair\@empty
\let\@phd@email\@empty
\let\@phd@homepage\@empty
\let\@phd@number\@empty
\let\@phd@phone\@empty
\let\@phd@referee\@empty
\let\@phd@rights\@empty
\let\@phd@phdcoord\@empty
\let\@phd@supervisor\@empty
\let\@phd@titleabstract\@empty
\newcommand*{\phdcoord}[1]{\gdef\@phd@phdcoord{#1}}
\newcommand*{\supervisor}[1]{\gdef\@phd@supervisor{#1}}
\newcommand*{\referee}[1]{\gdef\@phd@referee{#1}}
\newcommand*{\chair}[1]{\gdef\@phd@chair{#1}}
\newcommand*{\phdnumber}[1]{\gdef\@phd@number{#1}}
\newcommand{\rights}[1]{\gdef\@phd@rights{#1}}
\newcommand{\email}[1]{\gdef\@phd@email{#1}}
\newcommand{\homepage}[1]{\gdef\@phd@homepage{#1}}
\newcommand{\telephone}[1]{\gdef\@phd@phone{#1}}
\newcommand{\titleabstract}[1]{\gdef\@phd@titleabstract{#1}}
\newcommand*\printindex{\InputIfFileExists{\jobname.ind}{}
{\ClassWarningNoLine{PhdThesis}{No file \jobname.ind.}}}
\newcommand*{\maketitlepage}{
\begin{titlepage}%
\let\footnotesize\small
\let\footnoterule\relax
\null%\vfil
\begin{center}%
\vskip -2cm
\textsc{\@phd@university}\par
\vskip 2em plus .25em minus .25em
{\large\textsc{\thesisname\ifx\@phd@number\@empty\else:
\@phd@number\fi}\par}
\vskip 2em
{\LARGE\bfseries\@title\par}\vskip2em%
{\large\begin{tabular}[t]{c}%
\@author
\end{tabular}}\par
\ifx\@phd@titleabstract\@empty\else
\vskip.5em plus.25em minus.25em
\vfill
\begin{minipage}[t]{0.87\textwidth}
\textbf{\abstractname.} \@phd@titleabstract
\end{minipage}\par
\vskip.5em plus.25em minus.25em
\fi%
\vfill
{\large
\ifx\undefined\datename\else\datename~\fi
\@date\par}% Set date in \large size.
\end{center}%\par
\ifx\@phd@address\@empty\else
\par\vfil\begin{center}
\@phd@address
\end{center}
\fi%
\ifx\@thanks\@empty\else
\par\vfil\begin{center}
\@thanks
\end{center}
\fi%
\null
\end{titlepage}%
\thispagestyle{empty}
\setcounter{footnote}{0}%
\let\thanks\relax\let\maketitle\relax
\gdef\@thanks{}\gdef\@author{}\gdef\@title{}
}
\newcommand\chapterappendix{\par
\setcounter{section}{0}%
\setcounter{subsection}{0}%
\global\@phd@afterapptrue\global\let\@phd@thesection\thesection
\gdef\thesection{\thechapter.\@Alph\c@section}}
\newenvironment{abstract}
{\chapter*{\abstractname
\chaptermark{\abstractname}\sectionmark{\abstractname}}%
\thispagestyle{empty}}
{\thispagestyle{empty}\cleardoublepage\thispagestyle{empty}}
\newenvironment{acknowledgments}
{\chapter*{\acknowledgmentsname
\chaptermark{\acknowledgmentsname}\sectionmark{\acknowledgmentsname}}%
\thispagestyle{empty}}
{\thispagestyle{empty}\cleardoublepage\thispagestyle{empty}}
\newenvironment{approval}{
\begin{titlepage}
\vspace*{70pt}
\vskip 0pt plus.3fil
\begin{center}
}{
\end{center}
\vskip 0pt plus.6fil
\end{titlepage}
\thispagestyle{empty}
}
\newenvironment{dedication}{
\begin{titlepage}
\vspace*{70pt}
\vskip 0pt plus.3fil
\begin{flushright}
}{
\end{flushright}
\vskip 0pt plus.6fil
\thispagestyle{empty}
\end{titlepage}
\thispagestyle{empty}
}
\newenvironment{summary}
{\chapter*{\summaryname
\chaptermark{\summaryname}\sectionmark{\summaryname}}%
\thispagestyle{empty}}
{\thispagestyle{empty}\cleardoublepage\thispagestyle{empty}}
\newenvironment{chapterabstract}
{\normalfont\small\hrulefill\chapterabstractname\hrulefill%\bigskip
\quotation\noindent\small\ignorespaces}%
{\endquotation%\bigskip
\hrule\bigskip\noindent\ignorespaces}
\AtBeginDocument{
\providecommand*{\chapterabstractname}{~\abstractname~}
}
%%% Language Dependent and Shape Independent Names
\AtBeginDocument{\@ifpackageloaded{babel}{}{
\ClassWarningNoLine{PhdThesis}{The package babel is not loaded}
}}
\def\phd@setcaptions#1#2{%
\@ifpackageloaded{babel}{%
    \expandafter\addto\csname captions#1\expandafter\endcsname
    \expandafter{#2}%
}{#2}%
}
%%% Language Dependent Names
\def\phd@captions@american{%
\def\abstractname{Abstract}%
\def\acknowledgmentsname{Acknowledgments}%
\def\chairname{Chair}%
\def\conclusionsname{Conclusions}%
\def\indexname{Index}%
\def\introductionname{Introduction}%
\def\prefacename{Preface}%
\def\refereename{Referee}%
\def\summaryname{Summary}%
\def\supervisorname{Supervisor}%
\def\phdcoordname{PhD Coordinator}%
}
\let\phd@captions@english\phd@captions@american
\def\phd@captions@italian{%
\def\abstractname{Sommario}%
\def\acknowledgmentsname{Ringraziamenti}%
\def\chairname{Direttore}%
\def\conclusionsname{Conclusioni}%
\def\indexname{Indice Analitico}%
\def\introductionname{Introduzione}%
\def\prefacename{Prefazione}%
\def\refereename{Controrelatore}%
\def\summaryname{Riassunto}%
\def\supervisorname{Relatore}%
\def\phdcoordname{Coordinatore di Dottorato}%
}
\def\phd@captions@spanish{%
\def\abstractname{Resumen}%
\def\acknowledgmentsname{Agradecimientos}%
\def\chairname{Tutor}%
\def\conclusionsname{Conclusiones}%
\def\indexname{\'Indice alfab\'etico}%
\def\introductionname{Introducci\'on}%
\def\prefacename{Prefacio}%
\def\refereename{Revisor}%
\def\summaryname{Sumario}%
\def\supervisorname{Director}%
}
\def\phd@captions@french{%
\def\abstractname{Sommaire}%
\def\acknowledgmentsname{Remerciements}%
\def\chairname{Pr\'esident}%
\def\conclusionsname{Conclusions}%
\def\indexname{Index}%
\def\introductionname{Introduction}%
\def\prefacename{Pr\'eface}%
\def\refereename{Rapporteur}%
\def\summaryname{R\'esum\'e}%
\def\supervisorname{Directeur de th\`ese}%
}
%%% Patches to book.cls definitions
\renewcommand\tableofcontents{%
\if@twocolumn\@restonecoltrue\onecolumn\else\@restonecolfalse\fi
\chapter*{\contentsname
\chaptermark{\contentsname}\sectionmark{\contentsname}}%
\@starttoc{toc}\if@restonecol\twocolumn\fi
\if@mainmatter\ClassWarning{PhdThesis}
{Command \tableofcontents should be used in frontmatter mode}\fi}
\renewcommand\listoffigures{%
\if@twocolumn\@restonecoltrue\onecolumn\else\@restonecolfalse\fi
\chapter*{\listfigurename
\chaptermark{\listfigurename}\sectionmark{\listfigurename}}%
\@starttoc{lof}\if@restonecol\twocolumn\fi
\if@mainmatter\ClassWarning{PhdThesis}
{Command \listoffigures should be used in frontmatter mode}\fi}
\renewcommand\listoftables{%
\if@twocolumn\@restonecoltrue\onecolumn\else\@restonecolfalse\fi
\chapter*{\listtablename
\chaptermark{\listtablename}\sectionmark{\listtablename}}%
\@starttoc{lot}\if@restonecol\twocolumn\fi
\if@mainmatter\ClassWarning{PhdThesis}
{Command \listoftables should be used in frontmatter mode}\fi}
\AtBeginDocument{% Compatible with openbib?
\renewenvironment{thebibliography}[1]
{\chapter*{\bibname
\chaptermark{\bibname}\sectionmark{\bibname}}%
\typeout{\bibname.}%
\addcontentsline{toc}{chapter}{\protect\numberline{}\bibname}
\@mainmatterfalse
\list{\@biblabel{\@arabic\c@enumiv}}%
{\settowidth\labelwidth{\@biblabel{#1}}%
\leftmargin\labelwidth
\advance\leftmargin\labelsep
\@openbib@code
\usecounter{enumiv}%
\let\p@enumiv\@empty
\renewcommand\theenumiv{\@arabic\c@enumiv}}%
\sloppy
\clubpenalty4000
\@clubpenalty\clubpenalty
\widowpenalty4000%
\sfcode`\.\@m}
{\def\@noitemerr
{\@latex@warning{Empty `thebibliography' environment}}%
\endlist}
}
\renewenvironment{theindex}
{\if@twocolumn\@restonecolfalse\else\@restonecoltrue\fi
\if@openright\cleardoublepage\else\clearpage\fi
\columnseprule \z@
\columnsep 35\p@
\twocolumn[\vskip 7mm\@makeschapterhead{\indexname}]%
\chaptermark{\indexname}\sectionmark{\indexname}%
\typeout{\indexname.}%
\addcontentsline{toc}{chapter}{\protect\numberline{}\indexname}
\@mainmatterfalse
\thispagestyle{empty}\parindent\z@
\parskip\z@ \@plus .3\p@\relax
\let\item\@idxitem}
{\if@restonecol\onecolumn\else\clearpage\fi}
\renewcommand\appendix{\par
  \setcounter{chapter}{0}%
  \setcounter{section}{0}%
  \gdef\@chapapp{\appendixname}%
  \if@phd@afterintro
  \gdef\@phd@thechapter{\@Alph\c@chapter}
  \else
  \gdef\thechapter{\@Alph\c@chapter}
  \fi%
  \@mainmattertrue%this to behave like mainmatter without resetting page counter
  }
\def\@chapter[#1]#2{\ifnum \c@secnumdepth >\m@ne
\if@phd@afterintro
\global\@phd@afterintrofalse
\global\let\thechapter\@phd@thechapter
\fi
\if@phd@afterapp
\global\let\thesection\@phd@thesection
\global\@phd@afterappfalse
\fi
\if@mainmatter
\refstepcounter{chapter}
\typeout{\@chapapp\space\thechapter.}%
\addcontentsline{toc}{chapter}%
{\protect\numberline{\thechapter}#1}%
\else
\addcontentsline{toc}{chapter}{#1}%
\fi
\else
\addcontentsline{toc}{chapter}{#1}%
\fi
\chaptermark{#1}%
\addtocontents{lof}{\protect\addvspace{10\p@}}%
\addtocontents{lot}{\protect\addvspace{10\p@}}%
\if@twocolumn
\@topnewpage[\@makechapterhead{#2}]%
\else
\@makechapterhead{#2}%
\@afterheading
\fi}
%%% Load Shape Definition File and corresponding local .cfg
\providecommand*{\addto}[2]{%
  \ifx#1\@undefined
    \def#1{#2}%
  \else
    \ifx#1\relax
      \def#1{#2}%
    \else
      {\toks@\expandafter{#1#2}%
        \xdef#1{\the\toks@}}%
    \fi
  \fi
}
\expandafter\InputIfFileExists{\@phd@shapefile.sdf}{%
}{%
\ClassError{PhdThesis}{cannot load `\@phd@shapefile' shape document file}
{No help, sorry}
}
\InputIfFileExists{\@phd@shapefile.cfg}{
\ClassWarningNoLine{PhdThesis}{loaded LOCAL `\@phd@shapefile' page layout}
}{
}
\endinput
%%
%% End of file `PhdThesis.cls'.
