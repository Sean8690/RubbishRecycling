const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const TerserPlugin = require('terser-webpack-plugin');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;
const MomentLocalesPlugin = require('moment-locales-webpack-plugin');
const ESLintPlugin = require('eslint-webpack-plugin');
const ErrorOverlayPlugin = require('error-overlay-webpack-plugin');
const webpack = require('webpack');

const getPlugins = (env, devMode) => {
  const plugins = [
    new HtmlWebpackPlugin({
      title: 'CDD',
      template: 'src/index.html'
    }),
    new webpack.DefinePlugin({
      BUILD_NUMBER: JSON.stringify(!devMode ? env.buildNr : 'localDev')
    }),
    new MomentLocalesPlugin({
      localesToKeep: ['en-au']
    }),
    new ErrorOverlayPlugin(),
    new ESLintPlugin()
  ];

  if (env && env.analyze) {
    plugins.push(new BundleAnalyzerPlugin());
  }

  return plugins;
};

const config = (env, argv) => {
  const devMode = argv.mode !== 'production';

  let buildNr = 0;
  if (!devMode) {
    buildNr = env.buildNr;
    if (!buildNr) throw "env var 'buildNr' not set";
  }

  const config = {
    entry: {
      main: ['core-js', 'whatwg-fetch', path.join(__dirname, 'src')]
    },
    output: {
      publicPath: devMode ? '/' : `https://cf.infotrack.com.au/cdd/${buildNr}`,
      path: path.join(__dirname, 'dist'),
      filename: devMode ? '[name].js' : '[name]-[contenthash:8].js'
    },
    module: {
      rules: [
        {
          test: /\.(tsx|ts)?$/,
          exclude: '/node_modules/',
          use: ['babel-loader', 'ts-loader']
        },
        {
          test: /\.css$/,
          use: ['style-loader', 'css-loader']
        },
        { test: /\.(jpg|jpeg|png|woff|woff2|eot|ttf|svg)$/, loader: 'file-loader' }
      ]
    },
    resolve: {
      extensions: ['.tsx', '.ts', '.js']
    },
    optimization: {
      runtimeChunk: 'single',
      splitChunks: {
        chunks: 'all',
        maxInitialRequests: Infinity,
        minSize: 0,
        cacheGroups: {
          commons: {
            test: /node_modules/,
            /* Instead of bundling all the dependencies in a big vendors.js file,
               lets split a single (and small) js file for each dependency needed. */
            name(module) {
              const moduleName = module.context.match(
                /[\\/]node_modules[\\/](.*?)([\\/]|$)/
              )[1];
              return `npm.${moduleName.replace('@', '')}`;
            }
          }
        }
      },
      minimizer: [
        new TerserPlugin({
          parallel: true
        })
      ]
    },
    plugins: getPlugins(env, devMode),
    devtool: devMode ? 'source-map' : 'none'
  };

  if (devMode) {
    config.devServer = {
      contentBase: path.resolve(__dirname, 'src'),
      port: 3000,
      open: !env || !env.analyze,
      clientLogLevel: 'debug',
      logLevel: 'debug',
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Methods': 'GET, POST, PUT, DELETE, PATCH, OPTIONS',
        'Access-Control-Allow-Headers': '*'
      },
      host: 'localhost',
      disableHostCheck: true,
      proxy: {
        '/service/cdd/api/*': {
          target: 'http://localhost:5000'
        },
        '/Order/Show/*': {
          target: 'https://stagesearch.infotrack.com.au',
          changeOrigin: true
        },
        '/app/proxy-assets/*': {
          target: 'https://stagesearch.infotrack.com.au',
          changeOrigin: true
        },
        '/app/api/*': {
          target: 'https://stagesearch.infotrack.com.au',
          secure: false,
          logLevel: 'debug',
          changeOrigin: true
        },
        // 🚧🚧🚧🚧 Proxy rules required by TopBar to work in local development 🚧🚧🚧🚧
        '/micro/*': {
          target: 'https://stagesearch.infotrack.com.au',
          secure: false,
          changeOrigin: true
        },
        '/InMail/*': {
          target: 'https://stagesearch.infotrack.com.au',
          secure: false,
          changeOrigin: true
        },
        '/AutoComplete/*': {
          target: 'https://stagesearch.infotrack.com.au',
          secure: false,
          changeOrigin: true
        },
        '/app/api/*': {
          target: 'https://stagesearch.infotrack.com.au',
          secure: false,
          changeOrigin: true
        },
        '/service/imenu/api/*': {
          target: 'https://stagesearch.infotrack.com.au',
          secure: false,
          logLevel: 'debug',
          changeOrigin: true
        }
        // 🚧🚧🚧🚧 Proxy rules required by TopBar to work in local development 🚧🚧🚧🚧
      },
      historyApiFallback: true
    };
  }

  return config;
};

module.exports = config;
