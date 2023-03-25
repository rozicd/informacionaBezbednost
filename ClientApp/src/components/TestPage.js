import React, { Component } from 'react';
import Api from "../Api.js"
export class Test extends Component {
    static displayName = Test.name;

    constructor(props) {
        super(props);
        this.state = { currentCount: 0, testovi: []};
        this.incrementCounter = this.incrementCounter.bind(this);
    }

    incrementCounter() {
        console.log("lizmud")
        const api = new Api();
        const fetchUser = () => {
            api.getRacq()
                .then((response) => {
                    this.setState({ testovi: response.data });
                    console.log(response);
                })
                .catch((err) => console.log(err));
        };
        fetchUser();
    }

    render() {
        const { testovi } = this.state;
        return (

            <div>
                <h1>Test</h1>

                <p>This is a simple example of Test.</p>

                <p aria-live="polite">Current count: <strong>{this.state.currentCount}</strong></p>

                <button className="btn btn-primary" onClick={this.incrementCounter}>Increment</button>
                <table className='table table-striped' aria-labelledby="tabelLabel">
                    <thead>
                        <tr>
                            <th>Test</th>
                        </tr>
                    </thead>
                    <tbody>
                        {testovi.map(test =>
                            <tr key={test}>
                                <td>{test}</td>

                            </tr>
                        )}
                    </tbody>
                </table>
            </div>

        );
    }
}
