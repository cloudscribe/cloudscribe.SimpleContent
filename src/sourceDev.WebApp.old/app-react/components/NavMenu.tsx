import * as React from 'react';
import { Link, NavLink } from 'react-router-dom';

export class NavMenu extends React.Component<{}, {}> {
    public render() {
        return <div className='navbar navbar-expand-lg navbar-dark bg-dark'>
                <button type='button' className='navbar-toggler' data-toggle='collapse' data-target='.navbar-spa'>
                    <span className='navbar-toggler-icon'></span>
                </button>
                <div className='navbar-collapse collapse navbar-spa'>
                    <ul className='nav navbar-nav'>
                        <li className='nav-item'>
                            <NavLink to={ '/' } exact className='nav-link' activeClassName='active'>
                            <span className='fas fa-home'></span> Hello
                            </NavLink>
                        </li>
                        <li className='nav-item'>
                        <NavLink to={'/counter'} className='nav-link' activeClassName='active'>
                            <span className='fas fa-bolt'></span> Counter
                            </NavLink>
                        </li>
                        <li className='nav-item'>
                        <NavLink to={'/fetchdata'} className='nav-link' activeClassName='active'>
                            <span className='fas fa-th-list'></span> Fetch data
                            </NavLink>
                        </li>
                    </ul>
                </div>
        </div>;
    }
}
