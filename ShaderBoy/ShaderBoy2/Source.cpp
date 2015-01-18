#include <SFML/Graphics.hpp>

#include <iostream>

#include <direct.h>
#define GetCurrentDir _getcwd

struct ScreenInfos {
	float width;
	float height;
};

int main()
{
	ScreenInfos si = {1024.0f,786.0f};

	sf::VideoMode videoMode = sf::VideoMode(si.width, si.height);
	sf::RenderWindow window(videoMode, "Shaderboy");


	sf::Shader shader;
	bool error = false;
	if(!shader.loadFromFile("Shaders/basic.vs","Shaders/basic.fs")){
		std::cout<<"Error When loading basic"<<std::endl;
		error = true;
	}

	shader.setParameter("iResolution",si.width, si.height);
	
	sf::RectangleShape shape(sf::Vector2f(si.width,si.height));
    shape.setFillColor(sf::Color::Green);


	float globalTime = 0.;

	sf::Clock clock;
    while (window.isOpen())
    {
        sf::Event event;
        while (window.pollEvent(event))
        {
            if (event.type == sf::Event::Closed)
                window.close();
        }

		globalTime += clock.getElapsedTime().asSeconds();
		clock.restart();

		shader.setParameter("iGlobalTime",globalTime);

        window.clear();
        window.draw(shape,&shader);
        window.display();
    }

    return 0;
}