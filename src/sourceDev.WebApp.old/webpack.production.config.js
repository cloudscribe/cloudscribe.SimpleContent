const webpack = require("webpack");
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const merge = require('webpack-merge');
const webPackStrip = require('strip-loader');
const CompressionPlugin = require('compression-webpack-plugin');
const devConfig = require('./webpack.config.js');

const stripLoader = {
    test: [/\.js$/],
    exclude: /node_modules/,
    loader: webPackStrip.loader('console.log')
}

var devFiltered = devConfig;
devFiltered.plugins = []; //remove dev plugins, we don't want to merge with them we want to replace them

const overrides = {
    output: {
        filename: '[name].bundle.min.js',
    },
    module: {
        loaders: [stripLoader],
    },
    plugins: [
        new webpack.DefinePlugin({ // <-- key to reducing React's size
            'process.env': {
                'NODE_ENV': JSON.stringify('production')
            }
        }),
        new ExtractTextPlugin({
            filename: '[name].bundle.min.css',
            allChunks: true,
        }),
        new webpack.LoaderOptionsPlugin({
            minimize: true
        }),
        new webpack.optimize.UglifyJsPlugin(),
        new CompressionPlugin({   
            asset: "[path].gz[query]",
                algorithm: "gzip",
                test: /\.js$|\.css$|\.html$/,
                threshold: 10240,
                minRatio: 0.8
            })
    ]
      
};

const merged = merge(devFiltered, overrides);

module.exports = merged;
