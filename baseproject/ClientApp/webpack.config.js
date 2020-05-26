const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const CompressionPlugin = require('compression-webpack-plugin');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');
const OptimizeCSSAssetsPlugin = require('optimize-css-assets-webpack-plugin');


module.exports = (env = {}, argv = {}) => {

    const isProd = argv.mode === 'production';

    const config = {
        mode: argv.mode || 'development', // we default to development when no 'mode' arg is passed

        optimization: {
            minimizer: [
                new UglifyJsPlugin({ cache: true, parallel: true, sourceMap: false }),
                new OptimizeCSSAssetsPlugin({})
            ]
        },
        entry: {
            main: './src/main.js',
            // security: './src/security.js',
        },
        output: {
            filename: isProd ? 'bundle-[chunkHash].js' : '[name].js',
            path: path.resolve(__dirname, '../wwwroot/dist'),
            publicPath: "/"
        },
        plugins: [
            new MiniCssExtractPlugin({
                filename: isProd ? 'style-[contenthash].css' : 'main.css',
                path: path.resolve(__dirname, '../wwwroot/dist'),
                publicPath: "/"
            }),
            new CompressionPlugin({
                filename: '[path].gz[query]',
                algorithm: 'gzip',
                test: /\.js$|\.css$|\.html$|\.eot?.+$|\.ttf?.+$|\.woff?.+$|\.svg?.+$/,
                threshold: 10240,
                minRatio: 0.8
            }),
        ],
        module: {
            rules: [
                {
                    test: /\.js$/,
                    exclude: /node_modules/,
                    use: {
                        loader: "babel-loader"
                    }
                },
                {
                    test: /\.(sa|sc|c)ss$/,
                    use: [
                        'style-loader',
                        MiniCssExtractPlugin.loader,
                        'css-loader',
                        'sass-loader'
                    ]
                },
                {
                    test: /\.(png|jpg|gif|woff|woff2|eot|ttf|svg)$/,
                    loader: 'file-loader',
                    options: {
                        name: '[name].[hash].[ext]',
                        outputPath: '/assets/'
                    }
                }
            ]
        }
    };
    return config;
};