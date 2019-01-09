import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { AppContainer } from 'react-hot-loader';
import { Provider } from 'react-redux';
import { ConnectedRouter } from 'react-router-redux';
import { createBrowserHistory } from 'history';
import configureStore from './configureStore';
import { ApplicationState }  from './store';
import * as RoutesModule from './routes';
let routes = RoutesModule.routes;

// Create browser history to use in the Redux store
const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href')!;
const history = createBrowserHistory({ basename: baseUrl });

// Get the application-wide store instance, prepopulating with state from the server where available.
const initialState = (window as any).initialReduxState as ApplicationState;
const store = configureStore(history, initialState);

function renderApp() {
    // This code starts up the React app when it runs in a browser. It sets up the routing configuration
    // and injects the app into a DOM element.
    ReactDOM.render(
        <AppContainer>
            <Provider store={ store }>
                <ConnectedRouter history={ history } children={ routes } />
            </Provider>
        </AppContainer>,
        document.getElementById('react-app')
    );
}

document.addEventListener("DOMContentLoaded", function (event) {
    //console.log("DOM fully loaded and parsed");
    renderApp();
});



