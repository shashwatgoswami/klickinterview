const gulp = require("gulp");
const { ESLint } = require("eslint");
const eslint = new ESLint();
const concat = require('gulp-concat');
const less = require('gulp-less');

gulp.task('less', async (done) => {
    gulp.src(['src/style/*.less'])
        .pipe(less())
        .pipe(gulp.dest('dist/'))
    done();
});

gulp.task('eslint', async (done) => {
    const results = await eslint.lintFiles(["src/**/*.js"]);
    const formatter = await eslint.loadFormatter("stylish");
    const resultText = formatter.format(results);
    if (resultText) {
        done(resultText);
    } else {
        done();
    }
});

gulp.task('bundle-html', (done) => {
    gulp.src(['src/top.html', 'src/app/views/**/*.html', 'src/bottom.html'])
        .pipe(concat('index.html', { newLine: '\r\n' }))
        .pipe(gulp.dest('.'))
    done();
});

gulp.task('build', gulp.series(['less', 'eslint', 'bundle-html']));