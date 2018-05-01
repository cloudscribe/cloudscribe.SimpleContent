"use strict";

var gulp = require("gulp"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    gp_rename = require('gulp-rename'),
    uglify = require("gulp-uglify"),
    sass = require('gulp-sass'),
    sourcemaps = require('gulp-sourcemaps'),
    merge = require('merge-stream')
;

var config = {
    srcSassDir: './app-scss',
    cssOutDir: './sitefiles/s1/themes/custom1/wwwroot/css'
};

gulp.task('css', function () {
    return gulp.src(config.srcSassDir + '/style.scss')
    .pipe(sourcemaps.init())
    .pipe(sass({
        //outputStyle: 'compressed',
        includePaths: [
            config.srcSassDir + '/scss/'
           
        ],
    }))
    .pipe(sourcemaps.write())
        .pipe(gulp.dest(config.cssOutDir))
    .pipe(gp_rename('style.min.css'))
    .pipe(cssmin())
        .pipe(gulp.dest(config.cssOutDir))
    ;
});




gulp.task('default', ['css']);
