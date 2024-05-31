-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 31-05-2024 a las 23:10:26
-- Versión del servidor: 10.4.28-MariaDB
-- Versión de PHP: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmoapi`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `id` int(11) NOT NULL,
  `precio` double NOT NULL,
  `fechainicio` date NOT NULL,
  `fechaFin` date NOT NULL,
  `InquilinoId` int(11) NOT NULL,
  `InmuebleId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`id`, `precio`, `fechainicio`, `fechaFin`, `InquilinoId`, `InmuebleId`) VALUES
(1, 800000, '2024-05-29', '2024-06-29', 2, 63),
(2, 40000, '2024-03-08', '2024-04-08', 1, 60),
(11, 40000, '2024-03-15', '2024-04-15', 6, 71);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `id` int(11) NOT NULL,
  `direccion` varchar(40) NOT NULL,
  `ambientes` int(4) NOT NULL,
  `superficie` int(7) NOT NULL,
  `tipo` varchar(15) NOT NULL,
  `uso` varchar(15) NOT NULL,
  `precio` double NOT NULL,
  `disponible` tinyint(1) NOT NULL,
  `PropietarioId` int(11) NOT NULL,
  `ImagenUrl` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`id`, `direccion`, `ambientes`, `superficie`, `tipo`, `uso`, `precio`, `disponible`, `PropietarioId`, `ImagenUrl`) VALUES
(60, 'Tomas Jofre 976', 23, 2000, 'Casa', 'Comercio', 300000, 1, 5, 'Uploads/ab59e685-d62a-4399-8170-a21db879f6f6_casa1.jpg'),
(61, 'Av España 623', 12, 1000, 'Departamento', 'Vivienda', 40000, 1, 5, 'Uploads/3a38add7-720f-4d25-a1b4-b3ebf97ab958_Departamento1.jpg'),
(63, 'Av España 734', 12, 1000, 'Terreno', 'Deposito', 500000, 0, 3, ''),
(69, 'Europa 238', 1, 2000, 'Casa', 'Comercial', 3000000, 1, 3, 'Uploads/27d63386-862e-498d-ab84-a7fcd71e1809_depositphotos_115954550-stock-photo-home-exterior-with-garage-and.jpg'),
(70, 'San Martin 1080', 1, 2000, 'Departamento', 'Residencial', 100000, 0, 3, 'Uploads/123a5874-0926-4690-b3a3-d0fd01a0b45b_2015_DEPTO_JSMH_SMA_PHOTO_by_Paul_Rivera__03.jpg'),
(71, 'Corrientes 762', 3, 2009, 'Departamento', 'Residencial', 4000000, 0, 9, 'Uploads/f98e1b4a-edee-48bf-977c-6e414d8e17d9_departamento-piloto.png'),
(73, 'San Martin 1090', 2, 3000, 'Oficina', 'Comercial', 20000000, 0, 3, 'Uploads/1b8a3a47-dab2-4d13-8ced-36857262b75a_modern-homes-town-houses-600nw-2195504811.jpg'),
(74, 'Chacabuco 234', 1, 200, 'Casa', 'Comercial', 5000000, 0, 3, 'Uploads/6979ead0-4f69-40a9-bb8e-562809aa224b_IMG-20240528-WA0012.jpg');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `id` int(11) NOT NULL,
  `dni` int(9) NOT NULL,
  `apellido` varchar(40) NOT NULL,
  `nombre` varchar(40) NOT NULL,
  `direccion` varchar(50) NOT NULL,
  `telefono` varchar(200) NOT NULL,
  `email` varchar(200) NOT NULL,
  `NombreGarante` varchar(200) NOT NULL,
  `TelefonoGarante` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`id`, `dni`, `apellido`, `nombre`, `direccion`, `telefono`, `email`, `NombreGarante`, `TelefonoGarante`) VALUES
(1, 21345652, 'Velasquez', 'Maria', 'San Martin 2345', '2665012938', 'Maria@mail.com', 'Juan Alberto', '2665716832'),
(2, 27897345, 'Garay', 'Flor', 'Junin 345', '2664942701', 'Flor@mail.com', 'Gabriel Flores', '2665789000'),
(4, 32569087, 'Matias', 'Romero', 'Julio Roca 987', '2665050087', 'Matias@mail.com', 'Adriana Juarez', '2663876543'),
(5, 27863076, 'Zarate', 'Alicia', 'Junin 281', '2667861729', 'Alicia@mail.com', 'Pedro Fernandez', '2778234512'),
(6, 29008123, 'Romero', 'Ezequiel', 'Avenida Los Alcones 24', '1776234562', 'Ezequiel@mail.com', 'Kevin Navarro', '2776534280');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `id` int(11) NOT NULL,
  `nroPago` int(11) NOT NULL,
  `ContratoId` int(11) NOT NULL,
  `fechaPago` date NOT NULL,
  `importe` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`id`, `nroPago`, `ContratoId`, `fechaPago`, `importe`) VALUES
(1, 2, 1, '2024-05-30', 20000),
(2, 3, 1, '2024-05-31', 25000),
(5, 1, 2, '2024-05-15', 10000),
(6, 1, 11, '2024-04-15', 12000),
(7, 2, 11, '2024-05-16', 14000);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `Id` int(11) NOT NULL,
  `Dni` varchar(20) NOT NULL,
  `Apellido` varchar(40) NOT NULL,
  `Nombre` varchar(40) NOT NULL,
  `Telefono` varchar(40) NOT NULL,
  `Email` varchar(320) NOT NULL,
  `Clave` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`Id`, `Dni`, `Apellido`, `Nombre`, `Telefono`, `Email`, `Clave`) VALUES
(3, '38222345', 'Sanchez', 'Martin JUAN', '2664879812', 'Martin@mail.com', 'JxAUbYgsZrvhMhBSGk8tSHXGjyb83xS9M43I0aDodFs='),
(5, '32432098', 'Suarez', 'Jose ', '2664127895', 'Jose@mail.com', 'LcB4LWTLMqCVm+W+E3Do9WDgALfsHnPKTab2oIX63qA='),
(9, '27891345', 'Gimenes', 'Marcelo', '2665734561', 'Marcelo@mail.com', '2QwBjdju8/wNam7Q73FBWMgjpMcr41yxsiyylUV9uUU=');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idInquilino` (`InquilinoId`),
  ADD KEY `idInmueble` (`InmuebleId`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idPropietario` (`PropietarioId`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idAlquiler` (`ContratoId`),
  ADD KEY `ContratoId` (`ContratoId`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=75;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `contratos_ibfk_1` FOREIGN KEY (`InmuebleId`) REFERENCES `inmuebles` (`id`),
  ADD CONSTRAINT `contratos_ibfk_2` FOREIGN KEY (`InquilinoId`) REFERENCES `inquilinos` (`id`);

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `inmuebles_ibfk_1` FOREIGN KEY (`PropietarioId`) REFERENCES `propietarios` (`Id`);

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `pagos_ibfk_1` FOREIGN KEY (`ContratoId`) REFERENCES `contratos` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
