# Собирает все отчёты в папке reports
for i in `ls -d */`; do
    cd $i
    if [[ -d ./report ]]; then
        cd ./report
        pdflatex report.tex
        cp report.pdf ../../reports/"report_${i:0:-1}.pdf"
        cd ..
    fi
    cd ..
done
