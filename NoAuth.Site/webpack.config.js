var webpack = require('webpack');
var path = require('path');
require('es6-promise').polyfill();

module.exports = {
  entry: {
    login: './scripts/login.js'
  },
  devtool: process.env.WEBPACK_DEVTOOL || 'cheap-module-source-map',
  output: {
    path: path.join(__dirname, 'dist'),
    filename: '[name].entry.js',
    publicPath: 'dist'
  },
  module: {
	loaders: [{
		test: /\.css$/,
		loaders:["style-loader", "css-loader"]
	},
	{
		test: /\.less$/,
		loaders: ["style-loader", "css-loader!less-loader"]
	},
	{
		test: /(\.js)|(\.jsx)$/,
		exclude: /node_modules/,
		loaders: ['babel-loader?cacheDirectory=true,presets[]=es2015,presets[]=react']
	}
    ]
  },
  plugins: [
	new webpack.NoErrorsPlugin()
  ]
};
