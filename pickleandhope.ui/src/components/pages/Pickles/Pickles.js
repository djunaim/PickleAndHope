import React, { Component } from 'react';
import SinglePickle from '../../shared/SinglePickle/SinglePickle';
// import pickleData from '../../../helpers/data/pickleData';
import { getPickles } from '../../../helpers/data/pickleData';

class Pickles extends Component {
  state = {
    pickles: [
      // {id: 1, type: "dill", numberInStock: 12, size: "large", price: 5}
    ]
  }

  componentDidMount() {
    // this.getPickles();
    getPickles().then(pickles => this.setState({pickles: pickles}));
  }

  // getPickles = () => {
  //   pickleData.getPickles()
  //     .then((pickles) => this.setState({ pickles: pickles}))
  //     .catch((errFromGetPickles) => console.error(errFromGetPickles))
  // }

  render() {
    const { pickles } = this.state;

    return(
      pickles.map((pickle) => <SinglePickle key={pickle.id} pickle={pickle} />)
    ); 
  }
}

export default Pickles;