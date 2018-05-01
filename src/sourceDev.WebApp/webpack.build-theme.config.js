const path = require('path');
const webpack = require("webpack");
const ExtractTextPlugin = require("extract-text-webpack-plugin");

const config = {
    entry: {
        // each entry defines a bundle that will be produced
        'mainstyle': './app-scss/style.scss'
    },
    output: {
        filename: '[name].bundle.js',
        path: path.resolve(__dirname, 'sitefiles/s1/themes/custom1/wwwroot/dist/'),
        publicPath: '/dist/'
    },
    resolve: {
        modules: [path.join(__dirname,"./node_modules/")],
        extensions: [".tsx", ".ts", ".js", ".scss"]
    },
    devtool: 'source-map',
    externals: {
        // require("jquery") is external and available
        //  on the global var jQuery
        "jquery": "jQuery"
    },
    module: {
        rules: [
            {
                test: /\.css$/,
                use: ExtractTextPlugin.extract({
                    fallback: "style-loader",
                    use: "css-loader"
                }),
                exclude: /node_modules/
            },
            { 
                test: /\.(sass|scss)$/,
                use: ExtractTextPlugin.extract({
                    fallback: 'style-loader',
                    use: ['css-loader', 'sass-loader']
                })
                
            }
            ,
            {
                test: /\.tsx?$/,
                loader: 'ts-loader',
                exclude: /node_modules/
            },
        ]
    },
    plugins: [
        new webpack.HotModuleReplacementPlugin(),
        new webpack.NamedModulesPlugin(),
        new ExtractTextPlugin({
            filename: '[name].bundle.css',
            allChunks: true,
        }),
       
       
    ]
};

module.exports = config;