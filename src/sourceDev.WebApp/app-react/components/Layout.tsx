import * as React from 'react';
import { NavMenu } from './NavMenu';

export interface LayoutProps {
    children?: React.ReactNode;
}

export class Layout extends React.Component<LayoutProps, {}> {
    public render() {
        return <div className=''>
            <div className='row'>
                <div className='col-md-12'>
                    <NavMenu />
                </div>
            </div>
            <div className='row'>
                <div className='col-md-12'>
                    { this.props.children }
                </div>
            </div>
        </div>;
    }
}
