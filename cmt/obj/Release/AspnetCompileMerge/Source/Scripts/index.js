$('figure[slider=\'1\'].owl-carousel').owlCarousel({
    items: 1,
    loop: true,
    autoplayTimeout: 5000,
    autoPlay: true,
    nav: true,
    stopOnHover: true,
    responsive: {
        0: {
            items: 1
        },
        600: {
            items: 2
        },
        1000: {
            items: 3
        }

    }
});

$('div[slider=\'2\'].owl-carousel').owlCarousel({
    items: 3,
    loop: true,
    autoPlay: false,
    nav: true,
    stopOnHover: true,
    responsive: {
        0: {
            items: 1
        },
        600: {
            items: 2
        },
        1000: {
            items: 3
        }

    }
});